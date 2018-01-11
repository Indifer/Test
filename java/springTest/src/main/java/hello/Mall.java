package hello;

public class Mall {
    private long id;
    private String name;

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

    public Mall() {

    }

    public Mall(long id, String name) {

        this.id = id;
        this.name = name;
    }

    @Override
    public String toString() {
        return "Mall{" +
                "id=" + id +
                ", name='" + name + '\'' +
                '}';
    }
}
