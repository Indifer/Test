import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;

public class GenericsTest<T> {
    protected Class<T> clazz;

    public GenericsTest() {
        Type genType = getClass().getGenericSuperclass();
        if (genType instanceof ParameterizedType) {
            Type[] params = ((ParameterizedType) genType).getActualTypeArguments();
            clazz = (Class) params[0];
        }
    }

    public void theClassName() {
        if (clazz != null) {
            System.out.println("clazz.getName():" + clazz.getName());
        } else {
            System.out.println("clazz is unacknowledged");
        }
    }

}

class GenericsDuck extends GenericsTest<Duck> {

}