import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;

public class GenericsTest<T> {
    protected Class<T> clazz;

    public GenericsTest() {
        Type genType = getClass().getGenericSuperclass();
        Type[] params = ((ParameterizedType) genType).getActualTypeArguments();
        clazz = (Class) params[0];
    }
}

class GenericsDuck extends GenericsTest<Duck> {

    public void theClassName(){
        System.out.println("clazz.getName():" + clazz.getName());
    }

}