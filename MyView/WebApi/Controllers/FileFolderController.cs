using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Common.DBModel;
using Common.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace WebApi.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class FileFolderController : ControllerBase
    {
        private readonly DBContext dbc;

        public FileFolderController(DBContext dbContext)
        {
            dbc = dbContext;
        }

        [HttpGet]
        [Route("api/[controller]/{methodName:alpha}")]
        //[Route("api/[controller]/[action]")]
        public async Task<string> GetData(string methodName)
        {

            //return await Task.FromResult(MediaChart());
            switch (methodName)
            {
                case "???":
                    break;
                case "MediaChart":
                    return await Task.FromResult(MediaChartAsync());
            }
            return null;
        }
        
        private string MediaChartAsync()
        {
            var fileType = dbc.MFiletype.ToListAsync().Result;
            var fileInfo = dbc.TFileinfo.ToListAsync().Result;

            //var fileTypeList = await fileType;
            //var fileInfoList = await fileInfo;

            ChartView cv = new ChartView();


            PropertyInfo[] pi = cv.GetType().GetProperties();


            foreach(PropertyInfo p in pi)
            {
                if(fileType.Any(a => a.TypeName == "." + p.Name))
                {
                    string typeId = fileType.Where(w => w.TypeName == "." + p.Name).First().TypeId;
                    int value = fileInfo.Count(c => c.TypeId == typeId);
                    p.SetValue(cv, value);
                }
            }

            cv.total = fileInfo.Count;

            return JsonConvert.SerializeObject(cv);
        }
    }
}