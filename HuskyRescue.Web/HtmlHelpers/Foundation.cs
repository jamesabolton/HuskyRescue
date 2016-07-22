using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace HuskyRescue.Web.HtmlHelpers
{
	public static class Foundation
	{
		public static MvcHtmlString LabelTextBoxFor<TModel, TValue>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> modelProperty, object htmlLabelAttributes = null, object htmlTextBoxAttributes = null)
		{
			return LabelTextBoxFor(html, modelProperty, true, htmlLabelAttributes, htmlTextBoxAttributes);
		}

		public static MvcHtmlString LabelTextBoxFor<TModel, TValue>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> modelProperty, bool includeName, object htmlLabelAttributes = null, object htmlTextBoxAttributes = null)
		{
			var formatString = string.Empty;
			var memberExpression = modelProperty.Body as MemberExpression;
			if (memberExpression != null)
			{
				var displayFormatAttribute = memberExpression.Member.GetCustomAttributes(typeof(DisplayFormatAttribute), true);
				if (displayFormatAttribute.Length > 0)
				{
					formatString = ((DisplayFormatAttribute)displayFormatAttribute[0]).DataFormatString;
				}
			}

			var textbox = html.TextBoxFor(modelProperty, formatString, htmlTextBoxAttributes).ToString();


			if (!includeName)
			{
				var startIndex = textbox.IndexOf(" name=");
				var mid = textbox.IndexOf("\"", startIndex);
				var end = textbox.IndexOf("\"", mid + 1);
				textbox = textbox.Remove(startIndex, end - startIndex + 1);
			}

			var labelTag = new TagBuilder("label");
			var textboxName = ExpressionHelper.GetExpressionText(modelProperty);

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = string.Join(string.Empty, LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), textboxName), textbox);

			return new MvcHtmlString(labelTag.ToString());
		}

		public static MvcHtmlString LabelPasswordFor<TModel, TValue>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> modelProperty, object htmlLabelAttributes = null, object htmlPasswordAttributes = null)
		{
			var textbox = html.PasswordFor(modelProperty, htmlPasswordAttributes);

			var labelTag = new TagBuilder("label");
			var passwordName = ExpressionHelper.GetExpressionText(modelProperty);

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = string.Join(string.Empty, LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), passwordName), textbox.ToString());

			return new MvcHtmlString(labelTag.ToString());
		}

		public static MvcHtmlString LabelTextAreaFor<TModel, TValue>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> modelProperty, int rows = 2, int columns = 20, object htmlLabelAttributes = null, object htmlTextAreaAttributes = null)
		{
			var textarea = html.TextAreaFor(modelProperty, rows, columns, htmlTextAreaAttributes);

			var labelTag = new TagBuilder("label");
			var textareaName = ExpressionHelper.GetExpressionText(modelProperty);

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = string.Join(string.Empty, LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), textareaName), textarea.ToString());

			return new MvcHtmlString(labelTag.ToString());
		}

		public static MvcHtmlString LabelDropDownFor<TModel, TValue>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> modelProperty, IEnumerable<SelectListItem> selectList, object htmlLabelAttributes = null, object htmlDropDownAttributes = null)
		{
			var dropdown = html.DropDownListFor(modelProperty, selectList, null, htmlDropDownAttributes);

			var labelTag = new TagBuilder("label");
			var dropdownName = ExpressionHelper.GetExpressionText(modelProperty);

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = string.Join(string.Empty, LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), dropdownName), dropdown.ToString());

			return new MvcHtmlString(labelTag.ToString());
		}

		public static MvcHtmlString LabelCheckboxFor<TModel>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> modelProperty, object htmlLabelAttributes = null,
			object htmlCheckboxAttributes = null)
		{
			return LabelCheckboxFor(html, modelProperty, string.Empty, string.Empty, htmlLabelAttributes, htmlCheckboxAttributes);
		}

		public static MvcHtmlString LabelCheckboxFor<TModel>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> modelProperty, string checkboxLabelName, string id, object htmlLabelAttributes = null, object htmlCheckboxAttributes = null)
		{

			var tagId = string.IsNullOrEmpty(id)
				? ExpressionHelper.GetExpressionText(modelProperty)
				: id;

			var checkboxAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlCheckboxAttributes);

			if (!string.IsNullOrEmpty(id))
			{
				checkboxAttributes.Add("id", id);
				checkboxAttributes.Add("name", id);
			}

			var checkbox = html.CheckBoxFor(modelProperty, checkboxAttributes).ToString();

			if (!string.IsNullOrEmpty(id))
			{
				checkbox = checkbox.Replace("Selected", id);
			}

			var labelTag = new TagBuilder("label");

			//if (string.IsNullOrEmpty(checkboxLabelName))
			//{
			//	checkboxLabelName = ExpressionHelper.GetExpressionText(modelProperty);
			//}

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = (string.IsNullOrEmpty(checkboxLabelName)
				? LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), checkboxLabelName).ToString()
				: checkboxLabelName);

			labelTag.Attributes.Add("for", tagId);

			var index = checkbox.IndexOf(">") + 1;
			var checkboxInput = checkbox.Substring(0, index);
			var hiddenInput = checkbox.Substring(index);
			labelTag.InnerHtml = checkboxInput + labelTag.InnerHtml;
			return new MvcHtmlString(labelTag + hiddenInput);
		}

		public static MvcHtmlString LabelRadioButtonFor<TModel>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, string>> modelProperty, List<KeyValuePair<string, object>> options, object htmlLabelAttributes = null, object htmlRadioAttributes = null)
		{
			var radioName = ExpressionHelper.GetExpressionText(modelProperty);
			var radioAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlRadioAttributes);
			var radios = new List<string>();
			foreach (var option in options)
			{
				var idVal = radioName + option.Value;
				var radioLabel = new TagBuilder("label");
				radioLabel.Attributes.Add("for", idVal);
				if (radioAttributes.ContainsKey("class"))
				{
					radioLabel.AddCssClass((string)radioAttributes["class"]);
				}

				radioLabel.SetInnerText(option.Key);
				radioAttributes.Add("id", idVal);

				radios.Add(html.RadioButtonFor(modelProperty, option.Value, radioAttributes).ToHtmlString() + radioLabel);

				radioAttributes.Remove("id");
			}

			var labelTag = new TagBuilder("label");

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), radioName).ToString();

			return new MvcHtmlString(labelTag + string.Join(string.Empty, radios.ToArray()));
		}

		public static MvcHtmlString LabelRadioButtonFor<TModel>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, bool?>> modelProperty, List<KeyValuePair<string, object>> options, object htmlLabelAttributes = null, object htmlRadioAttributes = null)
		{
			var radioName = ExpressionHelper.GetExpressionText(modelProperty);
			var radioAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlRadioAttributes);
			var radios = new List<string>();
			foreach (var option in options)
			{
				var idVal = radioName + option.Value;
				var radioLabel = new TagBuilder("label");
				radioLabel.Attributes.Add("for", idVal);
				if (radioAttributes.ContainsKey("class"))
				{
					radioLabel.AddCssClass((string)radioAttributes["class"]);
				}

				radioLabel.SetInnerText(option.Key);
				radioAttributes.Add("id", idVal);

				radios.Add(html.RadioButtonFor(modelProperty, option.Value, radioAttributes).ToHtmlString() + radioLabel);

				radioAttributes.Remove("id");
			}

			var labelTag = new TagBuilder("label");

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), radioName).ToString();

			return new MvcHtmlString(labelTag + string.Join(string.Empty, radios.ToArray()));
		}

		public static MvcHtmlString LabelRadioButtonFor<TModel>(
			this HtmlHelper<TModel> html, Expression<Func<TModel, bool>> modelProperty, List<KeyValuePair<string, object>> options, object htmlLabelAttributes = null, object htmlRadioAttributes = null)
		{
			var radioName = ExpressionHelper.GetExpressionText(modelProperty);
			var radioAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlRadioAttributes);
			var radios = new List<string>();
			foreach (var option in options)
			{
				var idVal = radioName + option.Value;
				var radioLabel = new TagBuilder("label");
				radioLabel.Attributes.Add("for", idVal);
				if (radioAttributes.ContainsKey("class"))
				{
					radioLabel.AddCssClass((string)radioAttributes["class"]);
				}

				radioLabel.SetInnerText(option.Key);
				radioAttributes.Add("id", idVal);

				radios.Add(html.RadioButtonFor(modelProperty, option.Value, radioAttributes).ToHtmlString() + radioLabel);

				radioAttributes.Remove("id");
			}

			var labelTag = new TagBuilder("label");

			labelTag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlLabelAttributes));
			labelTag.InnerHtml = LabelHelper(ModelMetadata.FromLambdaExpression(modelProperty, html.ViewData), radioName).ToString();

			return new MvcHtmlString(labelTag + string.Join(string.Empty, radios.ToArray()));
		}

		private static MvcHtmlString LabelHelper(ModelMetadata metadata, string fieldName)
		{
			string labelText;
			var displayName = metadata.DisplayName;

			if (displayName == null)
			{
				var propertyName = metadata.PropertyName;

				labelText = propertyName ?? fieldName.Split(new[] { '.' }).Last();
			}
			else
			{
				labelText = displayName;
			}

			if (string.IsNullOrEmpty(labelText))
			{
				return MvcHtmlString.Empty;
			}

			return new MvcHtmlString(labelText);
		}
	}
}