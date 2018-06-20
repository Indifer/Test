import hello.*;
import javafx.scene.chart.PieChart;
import lombok.experimental.ExtensionMethod;
import lombok.experimental.var;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

import java.lang.reflect.Method;
import java.util.Properties;

public class Main {
    public static void main(String[] args) {

        ApplicationContext context = new ClassPathXmlApplicationContext("applicationContext.xml");

        // entity.xml
        User user = context.getBean("user", User.class);
        System.out.println(user);

        user = context.getBean("user2", User.class);
        System.out.println(user);

        user = context.getBean("user3", User.class);
        System.out.println(user);

        user = context.getBean("user4", User.class);
        System.out.println(user);

        Mall mall = context.getBean("mall2", Mall.class);
        System.out.println(mall);

        mall = context.getBean("mall", Mall.class);
        System.out.println(mall);

        Tuan tuan = context.getBean(Tuan.class);
        System.out.println(tuan);

        // Properties对象
        DataSource dataSource = context.getBean("dataSource", DataSource.class);
        System.out.println(dataSource);

        DataSource dataSource2 = context.getBean("dataSource2", DataSource.class);
        System.out.println(dataSource2);

        // autowire.xml
        Auto auto = context.getBean("auto", Auto.class);
        System.out.println(auto);

        // extends.xml
        Mall mallex2 = context.getBean("mall-ex2", Mall.class);
        System.out.println(mallex2);

        Mall mall2_ex2 = context.getBean("mall2-ex2", Mall.class);
        System.out.println(mall2_ex2);

        // scope.xml, 获取的bean实例不同
        User scope = context.getBean("user-scope", User.class);
        User scope2 = context.getBean("user-scope", User.class);
        System.out.println(" == : " + (scope == scope2));
        System.out.println("equals : " + scope.equals(scope2));

        // properties-config.xml
        DBConfig dbConfig = context.getBean("data", DBConfig.class);
        System.out.println(dbConfig);

    }

}
