package mapper;

import entity.Order;
import org.apache.ibatis.annotations.Insert;
import org.apache.ibatis.annotations.Select;

public interface OrderMapper {

    @Insert("insert into `order`(`item_id`,`title`,`status`) value(#{itemId},#{title},#{status})")
    public void insert(Order order);

    @Select("select * from `order` where id = #{id}")
    public Order getById(int id) throws Exception;

}
