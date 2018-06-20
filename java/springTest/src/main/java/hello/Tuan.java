package hello;

import java.util.List;
import java.util.Map;

public class Tuan {

    private List<User> users;
    private Map<String,User> userMap;

    public List<User> getUsers() {
        return users;
    }

    public void setUsers(List<User> users) {
        this.users = users;
    }

    public Map<String, User> getUserMap() {
        return userMap;
    }

    public void setUserMap(Map<String, User> userMap) {
        this.userMap = userMap;
    }

    @Override
    public String toString() {
        return "Tuan{" +
                "users=" + users +
                ", userMap=" + userMap +
                '}';
    }
}
