﻿using Microsoft.Extensions.Logging;

namespace SixRens.UI.MAUI.Services.ExceptionHandling
{
    public sealed class ExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> logger;
        private Page page;
        private readonly List<(Exception e, bool exitAfterDisplay)> exceptions;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            this.logger = logger;
            page = null;
            exceptions = new();
        }

        public void SetDisplayPage(Page page)
        {
            this.page = page;
            foreach (var e in exceptions)
            {
                var task = page.DisplayAlert("发生了错误：", e.e.Message, "确定");
                if (e.exitAfterDisplay)
                {
                    _ = task.ContinueWith((_) => {
                        Environment.Exit(0);
                    });
                }
            }
            exceptions.Clear();
        }

        public void Handle(
            Exception e,
            bool writeLog = true,
            bool display = true,
            bool exitAfterDisplay = true)
        {
            if (writeLog)
                logger.LogError(e, "");
            if (display)
            {
                if (page is null)
                    exceptions.Add((e, exitAfterDisplay));
                else
                {
                    var task = page.DisplayAlert("发生了错误：", e.Message, "确定");
                    if (exitAfterDisplay)
                    {
                        _ = task.ContinueWith((_) => {
                            Environment.Exit(0);
                        });
                    }
                }
            }
        }
    }
}
