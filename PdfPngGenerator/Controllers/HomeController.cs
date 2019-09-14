using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PdfPngGenerator.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly string HomePageHTML = @"
<html>

<head>
    <title>PDF/PNG Generator API</title>
   
    <link rel=""stylesheet"" href=""https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css"" integrity=""sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T"" crossorigin=""anonymous"">
</head>
<body class=""Container"">
      
    <div class=""jumbotron shadow-lg rounded border border-secondary"" style=""padding:50px"">
           
        <h1 class=""display-4 text-success ""><b> PDF/PNG Generator API</b></h1>
        <p class=""lead text-primary""><b>Download pdf / png snapshots for given url / html content</b></p>
        <hr class=""my-4"">
         <h3>Get png or pdf for given url</h3>  
        <br/>   
        <code> 
           <span class=""text-dark""> 
             CURL /api/getfile -method POST -dataType application/json <br/>
            -data <br/>
            { <br/>
                ""url"": ""https://github.com/SrinivasPrasadND/PDF-PNG-Generator/tree/master"", <br/>
            ""format"": ""png"", -- // supported values: [""png"", ""pdf""] <br/>
            ""headers"": { ""header1name"": ""header1value"", ""header2name"": ""header2value"" }, -- // optional parameter <br/>
                ""cookies"": ""Cookie1=Cookie1Value; Cookie2=Cookie2Value"" -- // optional parameter <br/>
            }
           </span> 
           <br/>
        </code>
        <hr class=""my-4""> 
          <h3>Get pdf for given HTML content</h3>  
        <br/>   
        <code> 
           <span class=""text-dark""> 
            CURL /api/generate -method POST -dataType application/json <br/>
            -data <br/>
            { <br/>
                ""htmlContent"": ""{Raw HTML Content}"", <br/>
            }
           </span> 
           <br/>
        </code>
        
    </div>
</body>

</html>
";
        public ActionResult Index()
        {
            return new ContentResult()
            {
                Content = HomePageHTML,
                ContentType = "text/html",
                StatusCode = 200
            };
        }
    }
}