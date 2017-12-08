import java.util.Scanner;

public class UnitsConverter {

    public static void test() {
//        System.out.println("input double for converter:");
//        Scanner scan = new Scanner(System.in);
//        String read = scan.nextLine();
//        scan.close();

//        double d = Double.parseDouble(read);
        double d = 11.11;
        for (Converter conv : Converter.values()) {
            System.out.println(conv.toString() + ":" + conv.performConversion(d));
        }

    }
}

enum Converter {

    KG("KG") {
        @Override
        double performConversion(double f) {
            return f *= 0.45;
        }
    },
    CARAT("carat") {
        @Override
        double performConversion(double f) {

            return f *= 2267.96;
        }

    },
    GMS("gms") {
        @Override
        double performConversion(double f) {
            return f *= 453.59;
        }
    }
    ;

    private final String symbol;

    Converter(String symbol) {
        this.symbol = symbol;
    }

    @Override
    public String toString() {
        return symbol.toString();
    }

    abstract double performConversion(double f);
}
