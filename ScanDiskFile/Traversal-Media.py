import os
import shutil
import sys
import time

import psutil
import pymysql
import json
import MyLog
import traceback

cfgFileName = 'AppSettings.json'
cfgFolder = 'Config'
createName = updateName = os.path.basename(sys.argv[0]).split('.')[0]
dirStartN = 'FN'
typeStartN = 'TN'
infoStartN = 'FIN'

#Setting情報取得
with open(cfgFolder + '/' + cfgFileName, 'r') as f:
    appSetting = json.load(f)

#Logの設定
log = MyLog.Logger(appSetting['LogFolder'] + '/Script.log', level='debug')

#获取系统所有盘符
def GetPartitions():
    diskParts = psutil.disk_partitions()
    log.logger.info('[%s]DiskPartitions in this computer', len(diskParts))

    startN = appSetting['DiskStart']
    i = 1
    for d in diskParts:
        dirverId = startN + '{:0>2d}'.format(i)
        createDateTime = updateDateTime = time.strftime('%Y-%m-%d %H:%M:%S', time.localtime())
        insStr = 'INSERT INTO M_DRIVER (DRIVER_ID, DRIVER_NAME, FILESYS_TYPE, SORT_KEY, CREATE_NAME, CREATE_DATETIME, UPDATE_NAME, UPDATE_DATETIME) SELECT %s, %s, %s, %s, %s, %s, %s, %s FROM DUAL WHERE NOT EXISTS (SELECT * FROM M_DRIVER WHERE DRIVER_ID = %s AND DRIVER_NAME = %s);'
        updStr = 'UPDATE M_DRIVER SET FILESYS_TYPE = %s, SORT_KEY = %s, UPDATE_NAME = %s, UPDATE_DATETIME = %s WHERE DRIVER_ID = %s AND DRIVER_NAME = %s;'

        insResult = cursor.execute(insStr, [dirverId, d.device, d.fstype, i, createName, createDateTime, updateName, updateDateTime, dirverId, d.device])
        if insResult == 0:
            cursor.execute(updStr, [d.fstype, i, updateName, updateDateTime, dirverId, d.device])

        i += 1

    conn.commit()

#查询所有文件和文件夹数，确定ID的位数
def CountFiles(driverList):
    global dirTotalLength
    global fileTotalLength

    dirTotal = 0
    fileTotal = 0
    for driver in driverList:
        dirCount = 0
        fileCount = 0
        for root, dirs, files in os.walk(driver):
            for dir in dirs:
                dirCount += 1

            for file in files:
                fileCount += 1

        log.logger.info('Driver:[{0}] has [{1}] folders.'.format(driver, dirCount))
        log.logger.info('Driver:[{0}] has [{1}] files.'.format(driver, fileCount))

        dirTotal += dirCount
        fileTotal += fileCount

    dirTotalLength = len(str(dirTotal))
    fileTotalLength = len(str(fileTotal))

    log.logger.info('Total [{0}] folders in this PC.'.format(dirTotal))
    log.logger.info('Total [{0}] files in this PC.'.format(fileTotal))

#获取列表所有项的FOLDER_ID
def GetFolderId(dirList, driverId):
    dirIdList = []
    for i in range(len(dirList)):
        if i == 0:
            cursor.execute('SELECT FOLDER_ID FROM M_FOLDER WHERE FOLDER_NAME = %s AND FOLDER_LEVEL = %s AND '
                           'DRIVER_ID = %s', [dirList[i], i + 1, driverId])
            dirIdList.append(cursor.fetchone()[0])
        else:
            cursor.execute(
                'SELECT FOLDER_ID FROM M_FOLDER WHERE FOLDER_NAME = %s AND FOLDER_LEVEL = %s AND DRIVER_ID = %s AND PARENT_ID = %s',
                [dirList[i], i + 1, driverId, dirIdList[-1]])
            dirIdList.append(cursor.fetchone()[0])
    return dirIdList

#フォルダーマスタ
def CreateDirMaster(name, level, parentId, driverId, sort):

    cursor.execute('SELECT * FROM M_FOLDER WHERE FOLDER_NAME = %s AND FOLDER_LEVEL = %s AND PARENT_ID = %s AND DRIVER_ID = %s', [name, level, parentId, driverId])
    folderInfo = cursor.fetchall()
    createDateTime = updateDateTime = time.strftime('%Y-%m-%d %H:%M:%S', time.localtime())

    if len(folderInfo) > 0:
        cursor.execute('UPDATE M_FOLDER SET SORT_KEY = %s, UPDATE_NAME = %s, UPDATE_DATETIME = %s WHERE FOLDER_NAME = '
                       '%s AND FOLDER_LEVEL = %s AND PARENT_ID = %s AND DRIVER_ID = %s', [sort, updateName,
                                                                                          updateDateTime, name,
                                                                                          level, parentId, driverId])
        log.logger.info('update data: [{0}]'.format(sort))
    else:
        cursor.execute('SELECT MAX(0 + RIGHT(FOLDER_ID, %s)) FROM M_FOLDER', [dirTotalLength])
        maxNum = cursor.fetchone()

        if maxNum[0] is None:
            maxId = 1
        else:
            maxId = maxNum[0] + 1

        id = dirStartN + str(int(maxId)).zfill(dirTotalLength)
        cursor.execute('INSERT INTO M_FOLDER(FOLDER_ID, FOLDER_NAME, FOLDER_LEVEL, PARENT_ID, DRIVER_ID, SORT_KEY, '
                       'CREATE_NAME, CREATE_DATETIME, UPDATE_NAME, UPDATE_DATETIME) VALUES(%s, %s, %s, %s, %s, %s, '
                       '%s, %s, %s, %s)', [id, name, level, parentId, driverId,
                                           sort, createName, createDateTime, updateName, updateDateTime])
        log.logger.info('insert data: [{0}], [{1}], [{2}], [{3}], [{4}], [{5}]'.format(id, name, level, parentId, driverId, sort))

def GetFileType():
    cursor.execute('SELECT TYPE_ID, TYPE_NAME FROM M_FILETYPE')
    result = cursor.fetchall()
    return result

def CreateFileRecord(name, typeId, path):
    #Create mappingKey
    mappingKey = ''
    pathList = path.split(os.path.sep)
    driver = pathList[0] + os.path.sep
    cursor.execute('SELECT DRIVER_ID FROM M_DRIVER WHERE DRIVER_NAME = %s', [driver])
    driverId = cursor.fetchone()[0]

    #文件不在磁盘根目录下的情况
    if len(pathList) > 2:
        dirList = pathList[1:-1]
        dirIdList = [str(driverId).replace('DN', '')]
        dirIdList.extend(GetFolderId(dirList, driverId))

        if len(dirIdList) > 1:
            folderIdList = [str(x).replace('FN', '') for x in dirIdList]
            mappingKey = '-'.join(folderIdList)
        else:
            raise Exception('Create Mapping-Key failed')
    else:
        #文件在磁盘根目录下的情况
        mappingKey = driverId.replace('DN', '')

    cursor.execute('SELECT * FROM T_FILEINFO WHERE FILE_NAME = %s AND MAPPING_KEY = %s', [name, mappingKey])
    createDateTime = updateDateTime = time.strftime('%Y-%m-%d %H:%M:%S', time.localtime())
    fileInfo = cursor.fetchone()

    if fileInfo is None:
        cursor.execute('SELECT MAX(0 + RIGHT(FILE_ID, %s)) FROM T_FILEINFO', [fileTotalLength])
        maxNum = cursor.fetchone()

        if maxNum[0] is None:
            maxId = 1
        else:
            maxId = maxNum[0] + 1

        id = infoStartN + str(int(maxId)).zfill(fileTotalLength)
        cursor.execute('INSERT INTO T_FILEINFO(FILE_ID, FILE_NAME, MAPPING_KEY, FILE_PATH, TYPE_ID, CREATE_NAME, CREATE_DATETIME, '
                       'UPDATE_NAME, UPDATE_DATETIME) VALUES(%s, %s, %s, %s, %s, %s, %s, %s, %s)',
                       [id, name, mappingKey, path, typeId, createName, createDateTime,
                        updateName, updateDateTime])
        log.logger.info(
            'insert data: [{0}], [{1}], [{2}], [{3}], [{4}]'.format(id, name, mappingKey, path, typeId))
    else:
        cursor.execute('UPDATE T_FILEINFO SET FILE_PATH = %s, TYPE_ID = %s, UPDATE_NAME = %s, UPDATE_DATETIME = %s '
                       'WHERE FILE_NAME = %s AND MAPPING_KEY = %s', [path, typeId, updateName, updateDateTime, name, mappingKey])
        log.logger.info('update data: [{0}], [{1}]'.format(path, typeId))

def dirTraversal(driverId, driverName):
    #获取媒体文件类型
    fileTypeList = GetFileType()

    #親フォルダー登録
    parentList = driverName.split(os.path.sep)
    CreateDirMaster(parentList[-2], len(parentList) - 2, 'null', driverId, 1)

    for root, dirs, files in os.walk(driverName):
        #folder处理
        i = 1
        for dir in dirs:
            dirPath = os.path.join(root, dir)
            dirDeep = len(dirPath.split(os.path.sep)) - 1
            startNum = len(parentList) - 2
            # if dirDeep > (len(parentList) - 1):
            #     startNum = (len(parentList) - 1)

            parentId = GetFolderId(dirPath.split(os.path.sep)[startNum:-1], driverId)[-1]
            CreateDirMaster(dir, dirDeep, parentId, driverId, i)
            print(dirPath)
            i += 1
        conn.commit()

        #file処理
        j = 1
        for file in files:
            filePath = os.path.join(root, file)
            dirName = filePath.split(os.path.sep)[-1]       #所属フォルダー
            extension = os.path.splitext(filePath)[1]       #扩展名

            for item in fileTypeList:
                if item[1] == extension:
                    #ファイル情報登録・更新
                    CreateFileRecord(file, item[0], filePath)
                    print(filePath)
                    j += 1

        conn.commit()

#DB接続
def SetConn():
    global conn, cursor

    log.logger.info('DB接続情報取得')
    with open(cfgFolder + '/' + appSetting['ConnConfigFile']) as f:
        connConfig = json.load(f)
        connProp = connConfig['ConnectionProperties']

    log.logger.info('DB接続開始......')
    conn = pymysql.connect(host=connProp['Server'],
                           user=connProp['Uid'],
                           password=connProp['Pwd'],
                           database=connProp['Database'],
                           charset=connProp['Charset'])

    cursor = conn.cursor()
    log.logger.info('DB接続成功')

def CloseConn():
    cursor.close()
    conn.close()

if __name__ == '__main__':
    log.logger.info('处理开始')
    try:
        SetConn()
        #CleanTable()
        #GetPartitions()

        targetDrivers = [('DN03', appSetting['MediaFilePath']),
                         ('DN03', appSetting['MovieFilePath'][0]),
                         ('DN04', appSetting['MovieFilePath'][1])]

        # 确定文件和文件夹ID的位数
        CountFiles([x[1] for x in targetDrivers])

        #ファイル情報登録
        for driver in targetDrivers:
            dirTraversal(driver[0], driver[1])

    except (Exception,BaseException) as e:
        exStr = traceback.format_exc()
        log.logger.error('发生异常：{0}'.format(exStr))
        print(exStr)
    finally:
        CloseConn()
        log.logger.info('处理结束')

