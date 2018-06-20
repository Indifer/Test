import java.io.File;
import java.io.IOException;
import java.io.Serializable;
import java.lang.annotation.Target;
import java.text.ParseException;
import java.util.HashMap;
import java.util.HashSet;
import java.util.concurrent.ExecutionException;

public class Main {

    public static void main(String[] args) throws Exception {

        String str1 = "hello";
        String str2 = new String("hello");

        System.out.println(str1 == str2);
        System.out.println(str1.equals(str2));

        //#region for-each

        System.out.println("for-each");

        int[] array = new int[]{1, 2, 3, 7, 8, 9};
        for (int item : array) {
            System.out.println(item);
        }

        //#endregion

        //#region enum

        System.out.println("enum");
        UnitsConverter.test();

        //#endregion

        //#region 内部类

        System.out.println("inner class");
        Outer outer = new Outer();
        Outer.Inner inner = outer.new Inner();

        inner.someMethod();
        Outer.StaticInner staticInner = new Outer.StaticInner();
        staticInner.someMethod();

        //#endregion

        //#region 继承

        System.out.println("extends");

        System.out.println("wings number:" + new Birds().WingsNumber);
        System.out.println("wings number:" + Birds.WingsNumber);

        new Duck("#ffffff").getClass().getAnnotation(Task.class);
        System.out.println("paws number:" + new Birds().PawsNumber);
        //System.out.println("paws number:" + Birds.PawsNumber);  error

        //#endregion

        //#region 匿名类

        System.out.println("匿名类");
        Person p1 = new Child();
        p1.eat();

        Person p2 = new Person() {
            @Override
            public void eat() {

            }

            @Override
            public String say() {
                return null;
            }
        };

        p2.eat();

        //#endregion

        //#region 泛型

        GenericsTest<Duck> genericsTest = new GenericsDuck();
        ((GenericsDuck) genericsTest).theClassName();

        (new GenericsTest<Birds>()).theClassName();

        //#endregion

        //#region 异步

        AsyncTaskTest asyncTaskTest = new AsyncTaskTest();

        asyncTaskTest.executorServiceTest();
        asyncTaskTest.completionServiceTest();
        asyncTaskTest.completableFutureTest();

        //#endregion

        //#region Exception

        System.out.println("Exception");
        boolean res = true;
        try {
            res = new Test().test();
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println(res);

        //#endregion
    }
}

class Birds implements Serializable {
    public static final int WingsNumber = 2;
    public final int PawsNumber = 2;
    public static Boolean isVertebrate;
    public static Boolean isOvipara;

    protected String color = null;
    protected Boolean canSwim;

    static {
        isVertebrate = true;
    }

    static {
        isOvipara = true;
    }

    @Task
    public Birds() {
        this(null);
    }

    @Task(123)
    public Birds(String color) {
        this.color = color;
        this.canSwim = false;
    }

    @Task(value = 123, desc = "456")
    public Birds(String color, Boolean canSwim) {
        this.color = color;
        this.canSwim = canSwim;
    }
}


class Duck extends Birds {

    public Duck() {
        this(null);
        //some code
    }


    /**
     * 构造函数
     *
     * @param color 默认颜色
     */
    public Duck(String color) {
        this(color, true);
        //some code
    }

    public Duck(String color, Boolean canSwim) {
        super(color, canSwim);
        //some code
    }
}

class Test {

    public Boolean test() throws TestException, IOException {


        boolean ret = true;
        try {
            String path = "";
            File file = new File(path);
            file.createNewFile();
            return ret;
        } catch (IOException ex) {
            System.out.println("catch:" + ex.getMessage());
            return false;
        } finally {
            System.out.println("finally");
            //return ret;
            throw new TestException("throw new exception");
        }
    }
}

class TestException extends Exception {
    public TestException(String message) {
        super(message);

    }

}
