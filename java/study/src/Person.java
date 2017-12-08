abstract class Person {
    public abstract void eat();

    public abstract String say();
}

class Child extends Person {
    @Override
    public void eat() {
        System.out.println("eat something");
    }

    @Override
    public String say() {
        return "Hello";
    }

}