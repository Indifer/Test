package springconsulconsumetest.demo.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.cloud.client.ServiceInstance;
import org.springframework.cloud.client.discovery.DiscoveryClient;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import springconsulconsumetest.demo.service.Users;

import java.util.List;

@RestController
@RequestMapping("/home")
public class Home {

    @Autowired
    private DiscoveryClient discoveryClient;

    @Autowired
    private Users users;

    @RequestMapping("/index")
    public String index() {
        return "index";
    }

    @RequestMapping("/home")
    public String home(String name) {
        return users.getHome(name);
    }

    @RequestMapping("/services")
    public List<String> services() {
        return discoveryClient.getServices();
    }

    @RequestMapping("/getInstances")
    public List<ServiceInstance> getInstances(String id) {
        return discoveryClient.getInstances(id);
    }

}
