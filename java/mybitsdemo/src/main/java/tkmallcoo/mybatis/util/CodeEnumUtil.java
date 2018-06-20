package tkmallcoo.mybatis.util;

import tkmallcoo.mybatis.Enum.IEnum;

public class CodeEnumUtil {
    public static <E extends Enum<?> & IEnum> E codeOf(Class<E> enumClass, int code) {
        E[] enumConstants = enumClass.getEnumConstants();
        for (E e : enumConstants) {
            if (e.getCode() == code) {
                return e;
            }
        }
        return null;
    }
}
