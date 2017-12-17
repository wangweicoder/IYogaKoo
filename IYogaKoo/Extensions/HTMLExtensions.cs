using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Text;

namespace System.Web.Mvc
{
    public static class HTMLExtensions
    {
        #region 单选框和复选框的扩展

        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxAndRadioFor<object, string>(name, selectList, false);
        }
        /// <summary>
        /// 复选框
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="selectList"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxFor(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBox(htmlHelper, name, selectList);
        }

        /// <summary>
        ///  根据列表输出checkbox,selValue为默认选中的项
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        public static MvcHtmlString CheckBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            return CheckBoxAndRadioFor<TModel, TProperty>(name, selectList, false);
        }
        /// <summary>
        /// 输出单选框和复选框
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <param name="selectList"></param>
        /// <param name="isRadio"></param>
        /// <param name="selValue"></param>
        /// <returns></returns>
        static MvcHtmlString CheckBoxAndRadioFor<TModel, TProperty>(
            string name,
            IEnumerable<SelectListItem> selectList,
            bool isRadio)
        {
            StringBuilder str = new StringBuilder();
            int c = 0;
            string check;
            string type = isRadio ? "Radio" : "checkbox";
            str.Append("<div class='radios'>");
            foreach (var item in selectList)
            {
                c++;
                if (item.Selected)
                {
                    check = "checked='checked'";
                }
                else
                {
                    check = string.Empty;
                }
                str.Append("<div class='radio'>");
                str.AppendFormat("<input type='{3}' value='{0}' name='{1}' id='{1}{2}' " + check + "/>", item.Value, name, c, type);
                str.AppendFormat("<label for='{0}{1}'>{2}</lable></div>", name, c, item.Text);
            }
            str.Append("</div>");
            return MvcHtmlString.Create(str.ToString());
        }


        public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxAndRadioFor<object, string>(name, selectList, true);
        }

        public static MvcHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            return CheckBoxAndRadioFor<TModel, TProperty>(name, selectList, true);
        }
        #endregion

        #region tag
        public static MvcHtmlString TagFor<T, TProperty>(this HtmlHelper<T> htmlHelper, Expression<Func<T, TProperty>> expression, IEnumerable<SelectListItem> selectList)
        {
            string name = ExpressionHelper.GetExpressionText(expression);
            string hidname = string.Empty;
            StringBuilder sb = new StringBuilder();
            sb.Append("<div class='tags'>");
            
            foreach (SelectListItem item in selectList)
            {
                if (item.Selected)
                {
                    hidname += item.Value + ",";
                }
                
                sb.Append("<span class='tag " + (item.Selected ? "selected" : "") + "' id='tag_" + item.Value + "'>" + item.Text + "</span>");
            }
            if (!string.IsNullOrEmpty(hidname))
            {
                hidname.Substring(0, hidname.Length-1);
            }
            sb.Append("<input type='hidden' name='" + name + "' id='" + name + "' value='" + hidname + "' /></div>");
            return MvcHtmlString.Create(sb.ToString());
        }
        #endregion
    }
}