using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using OrchardCore.DisplayManagement.Handlers;
using OrchardCore.DisplayManagement.ModelBinding;
using OrchardCore.DisplayManagement.Views;
using OrchardCore.Queries.Sql.ViewModels;

namespace OrchardCore.Queries.Sql.Drivers
{
    public class SqlQueryDisplayDriver : DisplayDriver<Query, SqlQuery>
    {
        private IStringLocalizer _stringLocalizer;

        public SqlQueryDisplayDriver(IStringLocalizer<SqlQueryDisplayDriver> stringLocalizer)
        {
            _stringLocalizer = stringLocalizer;
        }

        public override IDisplayResult Display(SqlQuery query, IUpdateModel updater)
        {
            return Combine(
                Shape("SqlQuery_SummaryAdmin", model =>
                {
                    model.Query = query;
                }).Location("Content:5"),
                Shape("SqlQuery_Buttons_SummaryAdmin", model =>
                {
                    model.Query = query;
                }).Location("Actions:2")
            );
        }

        public override IDisplayResult Edit(SqlQuery query, IUpdateModel updater)
        {
            return Shape<SqlQueryViewModel>("SqlQuery_Edit", model =>
            {
                model.Query = query.Template;
                model.ReturnDocuments = query.ReturnDocuments;

                // Extract query from the query string if we come from the main query editor
                if (string.IsNullOrEmpty(query.Template))
                {
                    updater.TryUpdateModelAsync(model, "", m => m.Query);

                    // The value is empty for new queries, remove any errors
                    if (string.IsNullOrEmpty(model.Query))
                    {
                        updater.ModelState.Clear();
                    }
                }

            }).Location("Content:5");
        }

        public override async Task<IDisplayResult> UpdateAsync(SqlQuery model, IUpdateModel updater)
        {
            var viewModel = new SqlQueryViewModel();
            if (await updater.TryUpdateModelAsync(viewModel, Prefix, m => m.Query, m => m.ReturnDocuments))
            {
                model.Template = viewModel.Query;
                model.ReturnDocuments = viewModel.ReturnDocuments;
            }

            return Edit(model, updater);
        }
    }
}
