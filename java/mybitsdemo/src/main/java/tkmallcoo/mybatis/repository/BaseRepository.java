package tkmallcoo.mybatis.repository;

import org.apache.ibatis.session.RowBounds;
import org.springframework.beans.factory.annotation.Autowired;
import tkmallcoo.mybatis.Entity.IEntity;
import tk.mybatis.mapper.entity.Example;
import tkmallcoo.mybatis.util.MapperBase;

import java.util.List;


public class BaseRepository<TEntity extends IEntity, TMapper extends MapperBase<TEntity>> {

    @Autowired
    private TMapper tmapper;

    /**
     * 根据主键字段进行查询，方法参数必须包含完整的主键属性，查询条件使用等号
     *
     * @param key
     * @return
     */
    public TEntity Get(Object key) {
        return tmapper.selectByPrimaryKey(key);
    }

    /**
     * 根据实体中的属性进行查询，只能有一个返回值，有多个结果是抛出异常，查询条件使用等号
     *
     * @param entity
     * @return
     */
    public TEntity Get(TEntity entity) {
        return tmapper.selectOne(entity);
    }

    /**
     * 根据Example条件进行查询
     *
     * @param object
     * @return
     */
    public TEntity Get(Example object) {
        return tmapper.selectOneByExample(object);
    }

    /**
     * 根据实体中的属性值进行查询，查询条件使用等号
     *
     * @param entity
     * @return
     */
    public List<TEntity> GetList(TEntity entity) {
        return tmapper.select(entity);
    }

    /**
     * 这个查询支持通过Example类指定查询列，通过selectProperties方法指定查询列
     *
     * @param example
     * @return
     */
    public List<TEntity> GetList(Example example) {
        return tmapper.selectByExample(example);
    }

    /**
     * 根据example条件和RowBounds进行分页查询
     *
     * @param example
     * @param rowBounds
     * @return
     */
    public List<TEntity> GetList(Example example, RowBounds rowBounds) {
        return tmapper.selectByExampleAndRowBounds(example, rowBounds);
    }

    /**
     * 根据实体属性和RowBounds进行分页查询
     *
     * @param entity
     * @param rowBounds
     * @return
     */
    public List<TEntity> GetList(TEntity entity, RowBounds rowBounds) {
        return tmapper.selectByRowBounds(entity, rowBounds);
    }

    /**
     * 查询全部结果，select(null)方法能达到同样的效果
     *
     * @return
     */
    public List<TEntity> GetList() {
        return tmapper.selectAll();
    }

    /**
     * 保存一个实体，null的属性也会保存，不会使用数据库默认值
     *
     * @param entity
     * @return
     */
    public int Insert(TEntity entity) {
        return tmapper.insert(entity);
    }

    /**
     * 保存一个实体，null的属性不会保存，会使用数据库默认值
     *
     * @param entity
     * @return
     */
    public int insertSelective(TEntity entity) {
        return tmapper.insertSelective(entity);
    }

    /**
     * 批量插入，该接口限制实体包含`id`属性并且必须为自增列
     *
     * @param entitylist
     * @return
     */
    public int insert(List<TEntity> entitylist) {
        return tmapper.insertList(entitylist);
    }

    /**
     * 根据主键更新实体全部字段，null值会被更新
     *
     * @param entity
     * @return
     */
    public int update(TEntity entity) {
        return tmapper.updateByPrimaryKey(entity);
    }

    /**
     * 根据Example条件更新实体`record`包含的全部属性，null值会被更新
     *
     * @param entity
     * @param example
     * @return
     */
    public int update(TEntity entity, Example example) {
        return tmapper.updateByExample(entity, example);
    }

    /**
     * 根据Example条件更新实体`record`包含的不是null的属性值
     *
     * @return
     */
    public int updateSelective(TEntity entity,Example example)
    {
         return  tmapper.updateByExampleSelective(entity,example);
    }

    /**
     * 根据主键更新属性不为null的值
     * @param entity
     * @return
     */
    public int updateSelective(TEntity entity)
    {
        return  tmapper.updateByPrimaryKeySelective(entity);
    }

    /**
     * 根据实体属性作为条件进行删除，查询条件使用等号
     * @param entity
     * @return
     */
    public int delete(TEntity entity)
    {
        return tmapper.delete(entity);
    }

    /**
     * 根据Example条件删除数据
     * @param example
     * @return
     */
    public int delete(Example example)
    {
        return  tmapper.deleteByExample(example);
    }

    /**
     * 根据主键字段进行删除，方法参数必须包含完整的主键属性
     * @param id
     * @return
     */
    public int delete(Object id)
    {
        return tmapper.deleteByPrimaryKey(id);
    }

    /**
     * 根据实体中的属性查询总数，查询条件使用等号
     * @param entity
     * @return
     */
    public  int count(TEntity entity)
    {
        return  tmapper.selectCount(entity);
    }

    /**
     * 根据Example条件进行查询总数
     * @param example
     * @return
     */
    public  int count(Example example)
    {
        return  tmapper.selectCountByExample(example);
    }
}
