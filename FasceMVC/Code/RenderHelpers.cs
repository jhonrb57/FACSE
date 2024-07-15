using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.IO;
using System.Text;

namespace FasceMVC.Code
{
    public static class RenderHelpers
    {
        /// <summary>
        /// Render an image button from the given params and associate it with an action of the current controller.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="controller">The current controller.</param>
        /// <param name="altText">The alt text (tool tip) for the button.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="action">The action of the current controller.</param>
        /// <param name="routeValues">The route values for the action.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="linkAttributes">The link attributes.</param>
        /// <returns>The HTML result.</returns>
        public static string ImageButton(Controller controller, string altText, string imageUrl,
            string action, object routeValues, object htmlAttributes = null, object linkAttributes = null)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                        new WebFormView(controller.ControllerContext, action),
                        controller.ViewData,
                        controller.TempData,
                        TextWriter.Null),
                new ViewPage());
            //
            return ImageButton(htmlHelper, altText, imageUrl, action, routeValues, htmlAttributes, linkAttributes).ToHtmlString();
        }

        /// <summary>
        /// Render an image button from the given params and associate it with an action of the current controller.
        /// </summary>
        /// <remarks>
        /// This is for using from a view!
        /// </remarks>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="altText">The alt text (tool tip) for the button.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="action">The action of the current controller.</param>
        /// <param name="routeValues">The route values for the action.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="linkAttributes">The link attributes.</param>
        /// <returns>The HTML result.</returns>
        public static MvcHtmlString ImageButton(this HtmlHelper htmlHelper, string altText, string imageUrl,
            string action, object routeValues, object htmlAttributes = null, object linkAttributes = null)
        {
            return ImageButton(htmlHelper, altText, imageUrl, null, action, routeValues, htmlAttributes, linkAttributes);
        }

        /// <summary>
        /// Render an image button from the given params and associate it with an action of the second controller specified by name.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="controller">The current controller.</param>
        /// <param name="altText">The alt text (tool tip) for the button.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="controllerName">The name of the second controller.</param>
        /// <param name="action">The action of the second controller.</param>
        /// <param name="routeValues">The route values for the action.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="linkAttributes">The link attributes.</param>
        /// <returns>The HTML result.</returns>
        public static string ImageButton(Controller controller, string altText, string imageUrl, string controllerName,
            string action, object routeValues, object htmlAttributes = null, object linkAttributes = null)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                        new WebFormView(controller.ControllerContext, action),
                        controller.ViewData,
                        controller.TempData,
                        TextWriter.Null),
                new ViewPage());
            //
            return ImageButton(htmlHelper, altText, imageUrl, controllerName, action, routeValues, htmlAttributes, linkAttributes).ToHtmlString();
        }

        /// <summary>
        /// Render an image button for from the given params and associate it with the action of the specified controller.
        /// </summary>
        /// <remarks>
        /// This is for using from a view!
        /// </remarks>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="altText">The alt text (tool tip) for the button.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="action">The action of the named controller.</param>
        /// <param name="routeValues">The route values for the action.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="linkAttributes">The link attributes.</param>
        /// <returns>The HTML result.</returns>
        public static MvcHtmlString ImageButton(this HtmlHelper htmlHelper, string altText, string imageUrl, string controllerName,
            string action, object routeValues, object htmlAttributes = null, object linkAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            //
            // Create an image tag builder for the given image.
            //
            var imageBuilder = new TagBuilder("img");
            imageBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
            imageBuilder.MergeAttribute("alt", altText);
            imageBuilder.MergeAttribute("title", altText);
            imageBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            //
            // Create a link tag builder that use the image tag builder!
            //
            var linkBuilder = new TagBuilder("a");
            if (linkAttributes == null || !(linkAttributes.ToString().Contains("href")))
            {
                linkBuilder.MergeAttribute("href", urlHelper.Action(action, controllerName, routeValues));
            }
            linkBuilder.MergeAttributes(new RouteValueDictionary(linkAttributes));
            linkBuilder.InnerHtml = imageBuilder.ToString(TagRenderMode.SelfClosing);
            //
            return MvcHtmlString.Create(linkBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Render an image  from the given params.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="altText">The alt text (tool tip) for the button.</param>
        /// <param name="imageUrl">The image URL.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>The HTML result.</returns>
        public static string Image(Controller controller, string action, string altText, string imageUrl,
            object htmlAttributes = null)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                    new WebFormView(controller.ControllerContext, action),
                    controller.ViewData,
                    controller.TempData,
                    TextWriter.Null),
                new ViewPage());
            //
            return Image(htmlHelper, altText, imageUrl, htmlAttributes).ToHtmlString();
        }

        public static MvcHtmlString Image(this HtmlHelper htmlHelper, string altText, string imageUrl, object htmlAttributes = null)
        {
            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            //
            // Create an image tag builder for the given image.
            //
            var imageBuilder = new TagBuilder("img");
            imageBuilder.MergeAttribute("src", urlHelper.Content(imageUrl));
            imageBuilder.MergeAttribute("alt", altText);
            imageBuilder.MergeAttribute("title", altText);
            imageBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            //
            return MvcHtmlString.Create(imageBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Render a dropdown list for an enum and the given params.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="controller"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="selectedValue"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        public static string EnumDropDownList<TEnum>(Controller controller, string name, string action, TEnum selectedValue, bool isReadOnly = false)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                        new WebFormView(controller.ControllerContext, action),
                        controller.ViewData,
                        controller.TempData,
                        TextWriter.Null),
                new ViewPage());
            //
            return EnumDropDownList<TEnum>(htmlHelper, name, action, selectedValue, isReadOnly).ToHtmlString();
        }

        /// <summary>
        /// Render a dropdown list for an enum and the given params.
        /// </summary>
        /// <remarks>
        /// This is for using from a view!
        /// </remarks>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="selectedValue"></param>
        /// <param name="isReadOnly"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDropDownList<TEnum>(this HtmlHelper htmlHelper, string name, string action, TEnum selectedValue, bool isReadOnly = false)
        {
            //
            // Create a list of SelectListItem from all values of the given enum.
            //
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
            IEnumerable<SelectListItem> items = from value in values
                                                select new SelectListItem
                                                {
                                                    Text = value.ToString(),
                                                    Value = value.ToString(),
                                                    Selected = (value.Equals(selectedValue))
                                                };
            //
            // Render the drop down list by using the list created above.
            //
            if (isReadOnly)
            {
                return MvcHtmlString.Create(htmlHelper.DropDownList(
                    name,
                    items,
                    null,
                    new
                    {
                        @disabled = "disabled",
                        style = "color: #999999;readonly:true;",
                    }
                    ).ToString());
            }
            else
            {
                return MvcHtmlString.Create(htmlHelper.DropDownList(
                    name,
                    items,
                    null,
                    new
                    {
                        onchange = string.Format("window.location='/{0}?value='+this.options[this.selectedIndex].value+ '&id='+ $(this).parent().parent()[0].id", action)
                    }
                    ).ToString());
            }

        }

        /// <summary>
        /// Render a custom checkbox from the given params.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="controller"></param>
        /// <param name="name"></param>
        /// <param name="action"></param>
        /// <param name="value"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string CustomCheckBox(Controller controller, string name, string action, string value, bool isReadOnly, object htmlAttributes = null)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                        new WebFormView(controller.ControllerContext, action),
                        controller.ViewData,
                        controller.TempData,
                        TextWriter.Null),
                new ViewPage());
            //
            return CustomCheckBox(htmlHelper, name, value, action, isReadOnly, htmlAttributes).ToHtmlString();
        }

        /// <summary>
        /// Render a custom checkbox from the given params.
        /// </summary>
        /// <remarks>
        /// This is for using from a view!
        /// </remarks>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="action"></param>
        /// <param name="isReadOnly"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString CustomCheckBox(this HtmlHelper helper, string name, string value, string action, bool isReadOnly, object htmlAttributes = null)
        {
            TagBuilder builder = new TagBuilder("input");
            //    
            if (Convert.ToInt32(value) == 1)
                builder.MergeAttribute("checked", "checked");
            //
            if (isReadOnly)
            {
                htmlAttributes = new
                {
                    @disabled = "disabled",
                    style = "color: #999999;readonly:true;",
                };
            }
            else
            {
                htmlAttributes = new
                {
                    style = "margin-left:auto; margin-right:auto;",
                    onchange = string.Format("window.location='/{0}?rowid=' +$(this).parent().parent()[0].id + '&value='+$(this).val()", action)
                };
            }
            //
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            builder.MergeAttribute("type", "checkbox");
            builder.MergeAttribute("name", name);
            builder.MergeAttribute("value", value);
            //
            return MvcHtmlString.Create(builder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Render an image from the given image stream load by ID from DB by using th action of the current controller
        /// and the others given params.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="controller"></param>
        /// <param name="altText"></param>
        /// <param name="action"></param>
        /// <param name="imageID"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string ImageFromStream(Controller controller, string altText,
            string action, int imageID, object htmlAttributes = null)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                        new WebFormView(controller.ControllerContext, action),
                        controller.ViewData,
                        controller.TempData,
                        TextWriter.Null),
                new ViewPage());
            //
            return ImageFromStream(htmlHelper, altText, action, imageID, htmlAttributes).ToHtmlString();
        }

        /// <summary>
        /// Render an image from the given image stream load by ID from DB by using th action of the current controller
        /// and the others given params.
        /// </summary>
        /// <remarks>
        /// This is for using from a view!
        /// </remarks>
        /// <param name="helper"></param>
        /// <param name="altText"></param>
        /// <param name="action"></param>
        /// <param name="imageID"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageFromStream(this HtmlHelper helper, string altText,
            string action, int imageID, object htmlAttributes = null)
        {
            return ImageFromStream(helper, altText, action, null, imageID, htmlAttributes);
        }

        /// <summary>
        /// Render an image from the given image stream load by ID from DB by using the action of a second controller specified by name 
        /// and the others given params.
        /// </summary>
        /// <remarks>
        /// This is for using from the current controller!
        /// </remarks>
        /// <param name="controller"></param>
        /// <param name="altText"></param>
        /// <param name="controllerName">The second controller name.</param>
        /// <param name="action"></param>
        /// <param name="imageID"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static string ImageFromStream(Controller controller, string altText, string controllerName,
            string action, int imageID, object htmlAttributes = null)
        {
            HtmlHelper htmlHelper = new HtmlHelper(
                new ViewContext(controller.ControllerContext,
                        new WebFormView(controller.ControllerContext, action),
                        controller.ViewData,
                        controller.TempData,
                        TextWriter.Null),
                new ViewPage());
            //
            return ImageFromStream(htmlHelper, altText, action, imageID, htmlAttributes).ToHtmlString();
        }

        /// <summary>
        /// Render an image from the given image stream load by ID from DB by using the action of a second controller specified by name 
        /// and the others given params.
        /// </summary>
        /// <remarks>
        /// This is for using from a view!
        /// </remarks>
        /// <param name="helper"></param>
        /// <param name="altText"></param>
        /// <param name="controllerName">The second controller name.</param>
        /// <param name="action"></param>
        /// <param name="imageID"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static MvcHtmlString ImageFromStream(this HtmlHelper helper, string altText, string controllerName,
            string action, int imageID, object htmlAttributes = null)
        {
            if (imageID > 0)
            {
                UrlHelper urlHelper = new UrlHelper(helper.ViewContext.RequestContext);
                //
                // Create an image tag builder for the given image.
                //
                var imageBuilder = new TagBuilder("img");
                imageBuilder.MergeAttribute("src", (controllerName == null
                    ? urlHelper.Action(action, new { ID = imageID })
                    : urlHelper.Action(action, controllerName, new { ID = imageID })));
                //
                if (altText != null)
                {
                    imageBuilder.MergeAttribute("alt", altText);
                    imageBuilder.MergeAttribute("title", altText);
                }
                //
                imageBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                //
                return MvcHtmlString.Create(imageBuilder.ToString(TagRenderMode.SelfClosing));
            }
            else
            {
                //
                // For invalid image ID return an empty string.
                //
                TagBuilder brTag = new TagBuilder("br");
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("");
                stringBuilder.Append(brTag.ToString(TagRenderMode.SelfClosing));
                //
                return MvcHtmlString.Create(stringBuilder.ToString());
            }
        }

    }
}