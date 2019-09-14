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
    [Route("api/generate")]
    [ApiController]
    public class PdfController : ControllerBase
    {

        [HttpPost]
        public async Task<Stream> Post()
        {
            string htmlContent = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync().ConfigureAwait(false);
            try
            {
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions
                {
                    IgnoreHTTPSErrors = true,
                    Headless = true,
                    Args = new string[] { "--no-sandbox" }
                }).ConfigureAwait(false);
                using (var page = await browser.NewPageAsync())
                {
                    await page.SetContentAsync(htmlContent).ConfigureAwait(false);

                    var result = await page.GetContentAsync().ConfigureAwait(false);

                    Stream pdfStream = await page.PdfStreamAsync(new PdfOptions() { Scale = 0.5M, PrintBackground = true }).ConfigureAwait(false);
                    await page.CloseAsync().ConfigureAwait(false);
                    await browser.CloseAsync().ConfigureAwait(false);
                    await browser.CloseAsync().ConfigureAwait(false);

                    return pdfStream;
                }

            }
            catch (Exception exMsg)
            {
                Stream stream = new MemoryStream();
                return stream;
            }
        }
    }
}