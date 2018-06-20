package hello.dto;

import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.stereotype.Component;

import java.time.LocalDateTime;

@Component
@Qualifier("dto_order")
public class Order {
    private String id;
    private String name;

    private LocalDateTime time;

    public String getId() {
        return id;
    }

    public String getName() {
        return name;
    }

    public LocalDateTime getTime() {
        return time;
    }

    public Order setId(String id) {
        this.id = id;
        return this;
    }

    public Order setName(String name) {
        this.name = name;
        return this;
    }

    public Order setTime(LocalDateTime time) {
        this.time = time;
        return this;
    }

    public Order(){
        System.out.println("Order1");
    }


    public Order(String name){

        System.out.println("Order2");
        this.name = name;
    }
}
