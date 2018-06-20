package com.example.repositorys;

import com.example.mapper.CityMapper;
import com.example.model.City;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import tkmallcoo.mybatis.repository.BaseRepository;

@Service
public class CityRepository extends BaseRepository<City,CityMapper> {
    @Autowired
    CityMapper cityMapper;

    /**
     * 根据ID查询city
     * @param id
     * @return
     */
    public City selectCityById(int id)
    {
        return cityMapper.selectCityById(id);
    }
}
