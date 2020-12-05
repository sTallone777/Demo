import os
import time
import timeit

# tmp = os.path.splitext('E:\Document\work\Python\FileTraversal\puretext')
# # # print(tmp[1])

# para1 = 12345
# para2 = 8
#
# print(str(para1).zfill(para2))
# print('ok')
t1 = timeit.default_timer()
time.sleep(2)
t2 =timeit.default_timer()
print('run time : %.2f' % (t2 - t1))