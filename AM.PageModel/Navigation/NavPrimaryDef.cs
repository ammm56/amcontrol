namespace AM.PageModel.Navigation
{
    /// <summary>
    /// 一级导航定义。
    /// </summary>
    public sealed class NavPrimaryDef
    {
        public NavPrimaryDef(string key, string displayName)
        {
            Key = key;
            DisplayName = displayName;
        }

        public string Key { get; private set; }

        public string DisplayName { get; private set; }
    }
}