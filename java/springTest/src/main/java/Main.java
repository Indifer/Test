import hello.*;
import org.springframework.context.ApplicationContext;
import org.springframework.context.support.ClassPathXmlApplicationContext;

public class Main {
    public static void main(String[] args) {

        ApplicationContext context = new ClassPathXmlApplicationContext("applicationContext.xml");
        User user = context.getBean("user", User.class);
        System.out.println(user);

        user = context.getBean("user2", User.class);
        System.out.println(user);

        user = context.getBean("user3", User.class);
        System.out.println(user);

        Mall mall = context.getBean(Mall.class);
        System.out.println(mall);

    }
}
