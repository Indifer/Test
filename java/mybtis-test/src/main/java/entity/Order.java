package entity;

import java.util.Date;

public class Order {
    private Integer id;
    private Integer itemId;
    private String title;
    private Integer status;
    private Date createTime;

    public Integer getId() {
        return id;
    }

    public Order setId(Integer id) {
        this.id = id;
        return this;
    }

    public Integer getItemId() {
        return itemId;
    }

    public Order setItemId(Integer itemId) {
        this.itemId = itemId;
        return this;
    }

    public String getTitle() {
        return title;
    }

    public Order setTitle(String title) {
        this.title = title;
        return this;
    }

    public Integer getStatus() {
        return status;
    }

    public Order setStatus(Integer status) {
        this.status = status;
        return this;
    }

    public Date getCreateTime() {
        return createTime;
    }

    public Order setCreateTime(Date createTime) {
        this.createTime = createTime;
        return this;
    }

    @Override
    public String toString() {
        return "Order{" +
                "id=" + id +
                ", itemId=" + itemId +
                ", title='" + title + '\'' +
                ", status=" + status +
                ", createTime=" + createTime +
                '}';
    }
}
