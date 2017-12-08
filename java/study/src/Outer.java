import java.util.Dictionary;
import java.util.HashMap;

public class Outer {
    private int size = 0;
    private static int staticSize = 3;

    public Inner createInner(){
        return this.new Inner();
    }

    public int getSize(){
        return size;
    }

    public class Inner {

        private int size = 1;
        public void someMethod() {
            size++;
            System.out.println(size);
            System.out.println(Outer.this.size);


            int y = 5;
            class Local{

                int x = 6;
                public void someMethod(){
                    System.out.println(size);
                    System.out.println(y);
                    System.out.println(x);
                }
            }

            new Local().someMethod();
        }
    }

    public static class StaticInner{

        public void someMethod() {
            System.out.println(staticSize);
        }

    }
}
