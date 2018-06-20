package hello;

public class DBConfig {

    private String user;
    private String pwd;
    private int port;

    public String getUser() {
        return user;
    }

    public void setUser(String user) {
        this.user = user;
    }

    public String getPwd() {
        return pwd;
    }

    public void setPwd(String pwd) {
        this.pwd = pwd;
    }

    public int getPort() {
        return port;
    }

    public void setPort(int port) {
        this.port = port;
    }

    @Override
    public String toString() {
        return "DBConfig{" +
                "user='" + user + '\'' +
                ", pwd='" + pwd + '\'' +
                ", port=" + port +
                '}';
    }

}
