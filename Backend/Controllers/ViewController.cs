﻿using System.Text.Json;
using AuthServer.Utils;
using Microsoft.AspNetCore.Mvc;

using SysFile = System.IO.File;

namespace AuthServer.Controllers
{
    public class ViewController : ControllerBase
    {
        InformationController ctrlInfo;
        MainController ctrlMain;

        public ViewController(IServiceProvider provider)
        {
            ctrlInfo = ActivatorUtilities.CreateInstance<InformationController>(provider);
            ctrlMain = ActivatorUtilities.CreateInstance<MainController>(provider);
        }

        [HttpGet("/authorize")]
        public async Task Test(string? app, string? redirect = null)
        {
            ctrlInfo.ControllerContext = ControllerContext;
            var index = new IndexFile();

            index.Add("settings", await ctrlInfo.Settings());

            var authorize = ctrlMain.TryAuthorize(app, redirect);
            if (!(authorize is SuccessResult)) index.Add("error", authorize);
            else index.Add("auth", authorize);

            await index.Write(Response);
        }
    }
}
