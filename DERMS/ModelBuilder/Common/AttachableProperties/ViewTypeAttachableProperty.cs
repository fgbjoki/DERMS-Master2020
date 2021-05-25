using ClientUI.Common.ViewType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientUI.Common.AttachableProperties
{
    public class ViewTypeAttachableProperty
    {
        public static readonly DependencyProperty ViewTypeProperty = DependencyProperty.RegisterAttached(
          "ViewType",
          typeof(ViewTypeEnum),
          typeof(ViewTypeAttachableProperty),
          new FrameworkPropertyMetadata(ViewTypeEnum.Default, FrameworkPropertyMetadataOptions.AffectsRender)
        );

        public static void SetViewType(UIElement element, ViewTypeEnum value)
        {
            element.SetValue(ViewTypeProperty, value);
        }
        public static ViewTypeEnum GetViewType(UIElement element)
        {
            return (ViewTypeEnum)element.GetValue(ViewTypeProperty);
        }
    }
}
