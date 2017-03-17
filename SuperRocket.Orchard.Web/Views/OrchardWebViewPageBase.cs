using Abp.Web.Mvc.Views;

namespace SuperRocket.Orchard.Web.Views
{
    public abstract class OrchardWebViewPageBase : OrchardWebViewPageBase<dynamic>
    {

    }

    public abstract class OrchardWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected OrchardWebViewPageBase()
        {
            LocalizationSourceName = OrchardConsts.LocalizationSourceName;
        }
    }
}