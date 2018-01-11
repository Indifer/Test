package hello.controllers;


import com.sun.org.apache.xpath.internal.operations.Or;
import hello.dto.Order;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.context.request.RequestAttributes;
import org.springframework.web.context.request.RequestContextHolder;
import org.springframework.web.context.request.ServletRequestAttributes;
import org.springframework.web.context.support.WebApplicationContextUtils;
import sun.misc.Request;

import javax.annotation.Resource;
import javax.servlet.http.HttpServletRequest;
import java.time.LocalDateTime;
import java.util.Date;

@Controller
@RequestMapping("/order")
public class OrderController {

    private Date date;

    public OrderController() {
        date = new Date();
        System.out.println("OrderController.....");
    }

    @Resource
    HttpServletRequest request;

    /**
     * @param model
     * @return
     */
    @RequestMapping("/index")
    public String index(Model model) {

        model.addAttribute("id", "abckdef");
        model.addAttribute("name", "团购订单");

        return "order/index";
    }

    /**
     * @param model
     * @return
     */
    @RequestMapping("/list")
    public String list(Model model) {

        Order order = new Order();
        order.setId("orderNo123");
        order.setName("订单1");
        model.addAttribute("order", order);

        return "order/list";
    }

    /**
     * @param order
     * @return
     */
    @ResponseBody
    @RequestMapping(value = "/post", method = {RequestMethod.POST})
    public Order post(@RequestBody() Order order) {
        order.setTime(LocalDateTime.now());
        return order;
    }

    @ResponseBody
    @RequestMapping(value = "/put", method = {RequestMethod.GET})
    public String put() {
        try {
            Thread.sleep(10000);
        } catch (Exception ex) {
        }
        return request.getQueryString();
    }

    @ResponseBody
    @RequestMapping(value = "/put2", method = {RequestMethod.GET})
    public String put2() {
        RequestAttributes requestAttr = RequestContextHolder.currentRequestAttributes();

        return ((ServletRequestAttributes) requestAttr).getRequest().getQueryString();
    }
}
