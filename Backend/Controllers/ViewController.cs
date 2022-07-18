using System.Text.Json;
using AuthServer.Utils;
using Microsoft.AspNetCore.Mvc;

using SysFile = System.IO.File;

namespace AuthServer.Controllers
{
    public class ViewController : ControllerBase
    {
        InformationController ctrlInfo;

        public ViewController(Config config)
        {
            ctrlInfo = new InformationController(config);
        }

        [HttpGet("/authorize")]
        public async Task Test()
        {
            ctrlInfo.ControllerContext = ControllerContext;
            var index = new IndexFile();



            // TODO: Делать запрос к /api/authorize
            index.Add("settings", await ctrlInfo.Settings());
            await index.Write(Response);
        }
    }
}
