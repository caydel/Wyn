using System.Threading.Tasks;

using Wyn.Mod.Admin.Core.Application.DictGroup.Dto;
using Wyn.Utils.Result;

namespace Wyn.Mod.Admin.Core.Application.DictGroup
{
    /// <summary>
    /// 数据字典服务
    /// </summary>
    public interface IDictGroupService
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResultModel> Query(DictGroupQueryDto dto);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResultModel> Add(DictGroupAddDto dto);
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultModel> Edit(int id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IResultModel> Update(DictGroupUpdateDto dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IResultModel> Delete(int id);

        /// <summary>
        /// 下拉选项
        /// </summary>
        /// <returns></returns>
        Task<IResultModel> Select();
    }
}
