package hello.controllers;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Scope;
import org.springframework.context.annotation.ScopedProxyMode;
import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.stereotype.*;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurerAdapter;

import javax.servlet.http.HttpServletRequest;
import java.util.Arrays;
import java.util.concurrent.CompletableFuture;
import java.util.function.Supplier;

@EnableAsync
@Controller
@RequestMapping("/home")
public class HomeController {

    @Autowired
    HttpServletRequest request2;

    @ResponseBody
    @RequestMapping("/index")
    public String index() throws Exception {
        return "home!";
    }

    @ResponseBody
    @RequestMapping("/test1")
    public String test1() throws Exception {
        //Thread.sleep(10000);
        return "test1!";
    }

    @ResponseBody
    @RequestMapping("/test2")
    @Async
    public CompletableFuture<String> test2() throws Exception {

//
        RequestAttributes requestAttributes = RequestContextHolder.getRequestAttributes();
        HttpServletRequest request = ((ServletRequestAttributes) requestAttributes).getRequest();
        System.out.println(request2.getQueryString());
        return CompletableFuture.supplyAsync(new Supplier<String>() {
            @Override
            public String get() {
                try {

                    return request.getQueryString();
                    //Thread.sleep(10000);
                } catch (Exception ex) {
                }
                return "test2!";
            }
        });

    }

}