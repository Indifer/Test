using EmitMapper;
using EmitMapper.Mappers;
using EmitMapper.MappingConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmitMapperConApp
{
    public static class ModelConverter
    {
        //#region GetInstance

        //private static Lazy<ModelConverter> _instance = new Lazy<ModelConverter>(() => new ModelConverter());
        ///// <summary>
        ///// 获取实例
        ///// </summary>
        //public static ModelConverter GetInstance
        //{
        //    get
        //    {
        //        return _instance.Value;
        //    }
        //}

        //#endregion

        private static Dictionary<string, ObjectsMapperBaseImpl> _dictMapping = new Dictionary<string, ObjectsMapperBaseImpl>();

        //private ModelConverter()
        //{
        //}

        /// <summary>
        /// object转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static TOut ConvertObject<TIn, TOut>(TIn inData)
        {
            Type tin = typeof(TIn);
            Type tout = typeof(TOut);

            string typeKey = string.Concat(tin, "|", tout);
            ObjectsMapper<TIn, TOut> mapper = null;
            if (!_dictMapping.ContainsKey(typeKey))
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TIn, TOut>();
                _dictMapping[typeKey] = mapper.MapperImpl;
            }
            else
            {
                mapper = new ObjectsMapper<TIn, TOut>(_dictMapping[typeKey]);
            }

            return mapper.Map(inData);
        }

        /// <summary>
        /// object转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static void Register<TIn, TOut>(DefaultMapConfig config = null)
        {
            Register<TIn, TOut>(config);
        }

        /// <summary>
        /// object转换
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="inData"></param>
        /// <returns></returns>
        public static void Register<TIn, TOut>(IMappingConfigurator config = null)
        {
            Type tin = typeof(TIn);
            Type tout = typeof(TOut);
            string typeKey = string.Concat(tin, "|", tout);
            ObjectsMapper<TIn, TOut> mapper = null;
            if (config == null)
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TIn, TOut>();
            }
            else
            {
                mapper = ObjectMapperManager.DefaultInstance.GetMapper<TIn, TOut>(config);
            }
            _dictMapping[typeKey] = mapper.MapperImpl;
        }

        #region 数据对象转换（待优化）
        /// <summary>
        /// 创建数据对象（待优化）
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        [Obsolete]
        public static TOut ConvertModel<TIn, TOut>(TIn data)
            where TIn : class, new()
            where TOut : class, new()
        {
            TOut result = new TOut();

            if (data == null)
                return null;
            Type tin = typeof(TIn);
            Type tout = typeof(TOut);

            var propertiesIn = tin.GetProperties();
            var propertiesOutDict = tout.GetProperties().ToDictionary(x => x.Name);

            foreach (var p in propertiesIn)
            {
                string name = p.Name;
                if (propertiesOutDict.ContainsKey(name))
                {
                    object obj = p.GetValue(data);
                    //propertiesOutDict[name].SetValue(result, obj);
                    if (p.PropertyType.IsClass && p.PropertyType != typeof(string))
                    {

                    }
                    else
                    {
                        propertiesOutDict[name].SetValue(result, obj);
                    }
                }
            }

            return result;
        }
        #endregion
    }
}
