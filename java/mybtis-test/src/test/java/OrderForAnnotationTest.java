import entity.Order;
import mapper.OrderMapper;
import org.apache.ibatis.session.SqlSession;
import org.apache.ibatis.session.SqlSessionFactory;
import org.apache.ibatis.session.SqlSessionFactoryBuilder;
import org.junit.Before;
import org.junit.Test;

import java.io.InputStream;

public class OrderForAnnotationTest {
    //定义 SqlSession
    SqlSession session = null;

    @Before
    public void init() {

        String resource = "mybatis-configuration.xml";

        InputStream inputStream = ItemForXmlTest.class.getClassLoader()
                .getResourceAsStream(resource);

        //构建sqlSession的工厂
        SqlSessionFactory sessionFactory = new SqlSessionFactoryBuilder().build(inputStream);
        //根据 sqlSessionFactory 产生 session
        session = sessionFactory.openSession();

    }

    @Test
    public void get() throws Exception {

        OrderMapper orderMapper = session.getMapper(OrderMapper.class);

        Order order = new Order().setTitle("yyy").setItemId(10).setStatus(2);
        orderMapper.insert(order);
        System.out.println(order);

        session.commit();

        order = orderMapper.getById(1);
        System.out.println(order);

        session.close();

    }

}
