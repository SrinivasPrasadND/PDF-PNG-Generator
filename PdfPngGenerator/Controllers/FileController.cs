using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp;

namespace PdfPngGenerator.Controllers
{
    [Route("api/getfile")]
    [ApiController]
    public class FileController : ControllerBase
    {

        [HttpPost]
        public async Task<Stream> Post([FromBody]RequestedData data)
        {
            try
            {
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    IgnoreHTTPSErrors = true,
                    Headless = true,
                    Args = new string[] { "--no-sandbox" }
                }).ConfigureAwait(false);
                using (var page = await browser.NewPageAsync().ConfigureAwait(false))
                {
                    await page.SetViewportAsync(new ViewPortOptions { Width = 1600, DeviceScaleFactor = 1 });
                    var hasHeader = data.Headers?.Any() ?? false;
                    var hasCookie = data.Cookies?.Any() ?? false;

                    if (hasHeader)
                    {
                        await page.SetExtraHttpHeadersAsync(data.Headers);
                    }

                    if (hasCookie)
                    {
                        var cookieArray = data.Cookies.Trim().Trim(';').Split(';');
                        var cookieCount = cookieArray.Length;
                        CookieParam[] cookiePrams = new CookieParam[cookieCount];
                        for (int i = 0; i < cookieCount; i++)
                        {
                            var cookieValues = cookieArray[i].Split('=');
                            cookiePrams[i] = new CookieParam
                            {
                                Url = data.Url,
                                Name = cookieValues[0].Trim(),
                                Value = cookieValues[1].Trim()
                            };
                        }

                        await page.SetCookieAsync(cookiePrams).ConfigureAwait(false);
                    }

                    Stream resultStream = new MemoryStream();

                    await page.GoToAsync(data.Url, WaitUntilNavigation.Networkidle0);
                    if (data.Format.Equals("pdf"))
                    {
                        
                        resultStream = await page.PdfStreamAsync(new PdfOptions() { PrintBackground = true,  Scale = 1 });
                        await page.CloseAsync().ConfigureAwait(false);
                    }
                    else if (data.Format.Equals("png"))
                    {
                        ScreenshotOptions screenshotOptions = new ScreenshotOptions() { FullPage = true };

                        resultStream = await page.ScreenshotStreamAsync(screenshotOptions);
                        await page.CloseAsync().ConfigureAwait(false);
                    }

                    await page.CloseAsync().ConfigureAwait(false);
                    await browser.CloseAsync().ConfigureAwait(false);
                    return resultStream;
                }
            }
            catch (Exception exMsg)
            {
                using (Stream stream = new MemoryStream())
                {
                    exMsg.Message.ToString();
                    return stream;
                }
            }
        }
    }

    public class RequestedData
    {
        public string Url { get; set; }
        public string Format { get; set; }
        public string Cookies { get; set; }
        public Dictionary<string, string> Headers { get; set; }

    }
}