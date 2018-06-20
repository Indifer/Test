package hello;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;

//@EnableAutoConfiguration
@SpringBootApplication
public class Application {
    public static void main(String[] args) throws Exception {

        //new B();
        System.out.println("嘎嘎嘎");
        SpringApplication.run(Application.class, args);

    }
}



//abstract class A {
//
//    public A() {
//        M();
//    }
//
//    public abstract void M();
//}
//
//class B extends A {
//
//    @Override
//    public void M() {
//        System.out.println("M");
//    }
//}