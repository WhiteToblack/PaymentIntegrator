#pragma checksum "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fd6b4d8c04db7d7b5b96fda33c789fa6e4d27d5f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_Index), @"mvc.1.0.view", @"/Views/Home/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\_ViewImports.cshtml"
using PaymentTest;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\_ViewImports.cshtml"
using PaymentTest.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fd6b4d8c04db7d7b5b96fda33c789fa6e4d27d5f", @"/Views/Home/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"27a2b26aac90393e0a492df180ea2a6c2356730f", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<PaymentTest.Models.PaymentModels.PayingUser>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
  
    ViewData["Title"] = "Home Page";

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"container-fluid\">\r\n        <div class=\"text-center\">\r\n            Kullanıcı Bilgileri\r\n        </div>\r\n        <div style=\"height:200px;\"></div>\r\n");
#nullable restore
#line 10 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
         using (Html.BeginForm("Index", "Payment", FormMethod.Post))
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("    <div class=\"form-row  justify-content-md-center\">\r\n");
            WriteLiteral("        <div class=\"form-group\">\r\n            <div class=\"md-form\">\r\n                ");
#nullable restore
#line 16 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
           Write(Html.TextBoxFor(u => u.User.Name, new { @class = "form-control", @placeholder = "Adı" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n            <div class=\"md-form \">\r\n                ");
#nullable restore
#line 19 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
           Write(Html.TextBoxFor(u => u.User.Surname, new { @class = "form-control", @placeholder = "Soyadı" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n");
            WriteLiteral("        <div class=\"form-group\">\r\n            <div class=\"md-form\">\r\n                <input type=\"text\" name=\"divison\" class=\"form-control\" placeholder=\"Bölüm\">\r\n            </div>\r\n            <div class=\"md-form\">\r\n                ");
#nullable restore
#line 28 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
           Write(Html.TextBoxFor(u => u.PaymentInformation.TotalAmount, new { @class = "form-control", @placeholder = "Ödenecek Tutar" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </div>\r\n        </div>\r\n        ");
#nullable restore
#line 31 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
   Write(Html.TextBoxFor(u => u.User.Id, new { @value=2, @style = "visibility:hidden" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        ");
#nullable restore
#line 32 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
   Write(Html.HiddenFor(u => u.PaymentInformation.CurrencyCode, new { @value="TRY" }));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("        <div class=\"row\">\r\n            <div class=\"col-md-6\">\r\n                <input type=\"submit\" class=\"btn-success\" value=\"Ödeme Sayasına Git\" />\r\n            </div>\r\n        </div>\r\n    </div>\r\n");
#nullable restore
#line 40 "C:\Users\MertCengiz\source\repos\PaymentManager\PaymentTest\Views\Home\Index.cshtml"
 }

#line default
#line hidden
#nullable disable
            WriteLiteral("    </div>\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<PaymentTest.Models.PaymentModels.PayingUser> Html { get; private set; }
    }
}
#pragma warning restore 1591
