using CommunityToolkit.Mvvm.ComponentModel;

namespace AM.ViewModel.ViewModels.Config
{
    /// <summary>
    /// 强类型配置编辑 ViewModel 基类。
    /// 用于承载当前有效配置对象的编辑模型。
    /// </summary>
    /// <typeparam name="TModel">配置模型类型。</typeparam>
    public abstract class ConfigEditorViewModelBase<TModel> : ObservableObject
        where TModel : class
    {
        /// <summary>
        /// 内部持有的配置模型。
        /// </summary>
        protected readonly TModel _model;

        /// <summary>
        /// 初始化配置编辑 ViewModel。
        /// </summary>
        /// <param name="model">配置模型。</param>
        protected ConfigEditorViewModelBase(TModel model)
        {
            _model = model;
        }

        /// <summary>
        /// 返回内部模型。
        /// </summary>
        public TModel GetModel()
        {
            return _model;
        }
    }
}