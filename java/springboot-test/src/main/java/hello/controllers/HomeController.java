package hello.controllers;

import org.springframework.scheduling.annotation.Async;
import org.springframework.scheduling.annotation.EnableAsync;
import org.springframework.stereotype.*;
import org.springframework.web.bind.annotation.*;

import java.util.concurrent.CompletableFuture;
import java.util.function.Supplier;

@EnableAsync
@Controller
@RequestMapping("/home")
public class HomeController {


    @RequestMapping("/index")
    @ResponseBody
    public String index() throws Exception {
        return "home!";
    }

    @RequestMapping("/test1")
    @ResponseBody
    public String test1() throws Exception {

        Thread.sleep(10000);
        return "test1!";
    }

    @RequestMapping("/test2")
    @ResponseBody
    @Async
    public CompletableFuture<String> test2() throws Exception {

        return CompletableFuture.supplyAsync(new Supplier<String>() {
            @Override
            public String get() {
                try {
                    Thread.sleep(10000);
                } catch (Exception ex) {
                }
                return "test2!";
            }
        });

    }

}