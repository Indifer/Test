package spring.consulclient.test;

import org.springframework.cloud.netflix.feign.FeignClient;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestParam;

@FeignClient("hello")
public interface HelloService {

    String home(@RequestParam(name = "name") String name);
}
