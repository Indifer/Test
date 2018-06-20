package springconsulservertest.springconsulservertest;


import com.orbitz.consul.AgentClient;
import com.orbitz.consul.Consul;
import org.springframework.beans.factory.DisposableBean;
import org.springframework.boot.ExitCodeGenerator;
import org.springframework.stereotype.Component;

@Component
public class TestImplDisposableBean implements DisposableBean, ExitCodeGenerator {

    @Override
    public void destroy() throws Exception {


        Consul consul = Consul.builder().withUrl("http://172.31.224.203:8500/").build(); // connect to Consul on localhost
        AgentClient agentClient = consul.agentClient();

        agentClient.deregister("users-9001");

        System.out.println("<<<<<<<<<<<我被销毁了......................>>>>>>>>>>>>>>>");
        System.out.println("<<<<<<<<<<<我被销毁了......................>>>>>>>>>>>>>>>");
    }

    @Override
    public int getExitCode() {

        return 5;
    }
}