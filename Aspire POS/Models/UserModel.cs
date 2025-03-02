namespace Aspire_POS.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Slug { get; set; }
        public AvatarUrls AvatarUrls { get; set; }
        public Meta Meta { get; set; }
        public bool IsSuperAdmin { get; set; }
        public WoocommerceMeta WoocommerceMeta { get; set; }
        public List<string> Roles { get; set; }
        public Links _Links { get; set; }
    }

    public class AvatarUrls
    {
        public string Size24 { get; set; }
        public string Size48 { get; set; }
        public string Size96 { get; set; }
    }

    public class Meta
    {
        public string WoocommerceLaunchYourStoreTourHidden { get; set; }
        public string WoocommerceComingSoonBannerDismissed { get; set; }
    }

    public class WoocommerceMeta
    {
        public string VariableProductTourShown { get; set; }
        public string ActivityPanelInboxLastRead { get; set; }
        public string ActivityPanelReviewsLastRead { get; set; }
        public string CategoriesReportColumns { get; set; }
        public string CouponsReportColumns { get; set; }
        public string CustomersReportColumns { get; set; }
        public string OrdersReportColumns { get; set; }
        public string ProductsReportColumns { get; set; }
        public string RevenueReportColumns { get; set; }
        public string TaxesReportColumns { get; set; }
        public string VariationsReportColumns { get; set; }
        public string DashboardSections { get; set; }
        public string DashboardChartType { get; set; }
        public string DashboardChartInterval { get; set; }
        public string DashboardLeaderboardRows { get; set; }
        public string HomepageLayout { get; set; }
        public string HomepageStats { get; set; }
        public string TaskListTrackedStartedTasks { get; set; }
        public string AndroidAppBannerDismissed { get; set; }
        public string LaunchYourStoreTourHidden { get; set; }
        public string ComingSoonBannerDismissed { get; set; }
    }

    public class Link
    {
        public string Href { get; set; }
        public TargetHints TargetHints { get; set; }
    }

    public class TargetHints
    {
        public List<string> Allow { get; set; }
    }

    public class Links
    {
        public List<Link> Self { get; set; }
        public List<Link> Collection { get; set; }
    }
}
