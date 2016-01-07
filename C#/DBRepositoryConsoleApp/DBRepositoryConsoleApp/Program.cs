using EntityFramework.Repository;
using Repository.IEntity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepositoryConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (EFContext content = new EFContext())
            {
                ProductSpecRepository rep = new ProductSpecRepository(content);
                var model = new ProductSpec { ID = 48, PropertyNames = "xxx" };
                rep.Update(model);

                rep.Save();
            }

            Console.ReadLine();
        }
    }

    public class ProductSpec : Repository.IEntity.IAutoIncr<long>
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 商品ID
        /// </summary>
        public long ProductID { get; set; }

        /// <summary>
        /// 商场ID
        /// </summary>
        public long MallID { get; set; }

        /// <summary>
        /// 商户ID
        /// </summary>
        public long ShopID { get; set; }

        /// <summary>
        /// 属性Ids | 2,3
        /// </summary>
        public string PropertyIds { get; set; }

        /// <summary>
        /// 属性名称 | 颜色,尺码
        /// </summary>
        public string PropertyNames { get; set; }

        /// <summary>
        /// 属性值Ids | 2,3
        /// </summary>
        public string PropertyValueIds { get; set; }

        /// <summary>
        /// 属性值名称 | 红色,170
        /// </summary>
        public string PropertyValueNames { get; set; }

        /// <summary>
        /// 库存数
        /// </summary>
        public long StoreCount { get; set; }

        /// <summary>
        /// 销售数(预留字段)
        /// </summary>
        public long SaleCount { get; set; }

        /// <summary>
        /// 商品规格编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }

        #region other

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 记录创建人ID
        /// </summary>
        public long CreatorID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人ID
        /// </summary>
        public long UpdatorID { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Updator { get; set; }

        /// <summary>
        /// 最后一次修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductSpec()
        {
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            Status = 1;
        }
    }

    public class ProductSpecRepository : EFRepository<ProductSpec, long>
    {
        public ProductSpecRepository(DbContext context)
            : base(context)
        {

        }
    }

    public abstract class ShoppingMallBaseRepository_EF<TEntity, TKey> : EFRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public ShoppingMallBaseRepository_EF(DbContext context)
            : base(context)
        {
        }


    }
}
