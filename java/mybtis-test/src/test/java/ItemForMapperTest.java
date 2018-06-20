import entity.Item;
import mapper.ItemMapper;
import mapper.OrderMapper;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.session.SqlSessionFactoryBuilder;
import org.junit.Assert;
import org.junit.Before;
import org.junit.Test;

import java.io.InputStream;
import java.util.HashMap;
import java.util.Hashtable;
import java.util.List;
import java.util.UUID;

public class ItemForMapperTest {

    //定义 SqlSession
    SqlSession session = null;

    @Before
    public void init() {

        String resource = "mybatis-configuration.xml";

        InputStream inputStream = ItemForMapperTest.class.getClassLoader()
                .getResourceAsStream(resource);

        //构建sqlSession的工厂
        SqlSessionFactory sessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
        //根据 sqlSessionFactory 产生 session
        session = sessionFactory.openSession();
    }

    @Test
    public void getListTest() throws Exception {


        ItemMapper itemMapper = session.getMapper(ItemMapper.class);
        List<Item> list = itemMapper.getList();
        for (Item item : list) {
            System.out.println(item);
        }
        session.close();
    }

//    @Test
//    public void getById() {
//        Item item = session.selectOne("item.ItemMapper.getById", 2);
//        System.out.println(item);
//        Assert.assertNotNull(null, item);
//        session.close();
//    }
//
//    @Test
//    public void insertTest() {
//
//        Item item = new Item().setName(UUID.randomUUID().toString());
//        session.insert("item.ItemMapper.add", item);
//        session.commit();
//        session.close();
//        System.out.println(item);
//    }
//
//    @Test
//    public void update() {
//
//    }

}
