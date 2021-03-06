#pragma checksum "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d2ab397e662ee8497a6e48e17bedbb2193e5844b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_CommandeDetailHistory), @"mvc.1.0.view", @"/Views/Home/CommandeDetailHistory.cshtml")]
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
#line 1 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\_ViewImports.cshtml"
using Front;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\_ViewImports.cshtml"
using Front.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d2ab397e662ee8497a6e48e17bedbb2193e5844b", @"/Views/Home/CommandeDetailHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9648054883d18bf98c8cacf842148b10bdfdbeee", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_CommandeDetailHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-block btn-light"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Home", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "CommandeHistory", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
  
    ViewData["Title"] = "FactureDetailHistory";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

<div class=""container"">

    <div class=""row"">
        <div class=""text-center"">
            <h1>Detail de la commande</h1>
        </div>
        <table class=""table table-hover"">
            <thead>
                <tr>
                    <th>Produit</th>
                    <th>#</th>
                    <th class=""text-center"">Prix</th>
                    <th class=""text-center"">Total</th>
                </tr>
            </thead>
            <tbody>

");
#nullable restore
#line 24 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                 foreach (var detailB in ViewBag.ListeDetailsCommandes)
                {
                    

#line default
#line hidden
#nullable disable
#nullable restore
#line 26 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                     foreach (var book in ViewBag.ListeBooks)
                    {
                        if (detailB.BookId == book.Id)
                        {

#line default
#line hidden
#nullable disable
            WriteLiteral("                            <tr data-bookid=\"");
#nullable restore
#line 30 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                                        Write(detailB.BookId);

#line default
#line hidden
#nullable disable
            WriteLiteral("\">\r\n                                <td class=\"col-md-9\"><h4><em>");
#nullable restore
#line 31 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                                                        Write(book.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</em></h4></td>\r\n\r\n                                <td class=\"col-md-1\" style=\"text-align: center\"> ");
#nullable restore
#line 33 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                                                                            Write(detailB.Quantity);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </td>\r\n                                <td class=\"col-md-1\" style=\"text-align: center\"> ");
#nullable restore
#line 34 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                                                                            Write(book.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </td>\r\n                                <td class=\"col-md-1\" style=\"text-align: center\"> ");
#nullable restore
#line 35 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                                                                            Write(detailB.PrixTotal);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </td>\r\n\r\n\r\n                            </tr>\r\n");
#nullable restore
#line 39 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                        }
                    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 40 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                     
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>   </td>\r\n                    <td>   </td>\r\n                    <td class=\"text-right\"><h4><strong>Total: </strong></h4></td>\r\n                    <td class=\"text-center text-danger\"><h4><strong>");
#nullable restore
#line 46 "C:\temp\asp-net-projects\final\eLibrairie\Front\Views\Home\CommandeDetailHistory.cshtml"
                                                               Write(ViewBag.PrixTotal);

#line default
#line hidden
#nullable disable
            WriteLiteral("€</strong></h4></td>\r\n                </tr>\r\n            </tbody>\r\n        </table>\r\n\r\n        <div class=\"col-sm-12  col-md-6\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d2ab397e662ee8497a6e48e17bedbb2193e5844b8559", async() => {
                WriteLiteral("Retour à la liste");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n        </div>\r\n\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
