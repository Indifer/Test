package hello;

import lombok.EqualsAndHashCode;

@EqualsAndHashCode
public class User {
    private long id;
    private String name;
    private int arg;

    public long getId() {
        return id;
    }

    public void setId(long id) {
        this.id = id;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getArg() {
        return arg;
    }

    public void setArg(int arg) {
        this.arg = arg;
    }

    public User() {

    }

    public User(long id, String name) {

        this.id = id;
        this.name = name;
    }

    public User(int arg, String name) {

        this.arg = arg;
        this.name = name;
    }

    @Override
    public String toString() {
        return "User{" +
                "id=" + id +
                ", name='" + name + '\'' +
                ", arg=" + arg +
                '}';
    }
}
