# 数据库操作模块

数据库应用模块，在Gentings中数据库使用ADO.NET进行封装，有点类似于Linq的写法；这也许不是最好的做法，不过比EntityFramework简单，而且在应用当中很少使用多种数据库的项目。在`Gentings.Data`中，主要包含了几个功能：CodeFirst代码迁移，`IDatabase`原始ADO.NET操作，以及`IDbContext<TModel>`模型对象操作。

## CodeFirst代码迁移

在框架中，使用CodeFirst来建立数据库和更改数据表结构。在定一个实体类后，需要实现基类：`Gentings.Data.Migrations.DataMigration`，在实现类里定义一个表格的相关操作。

实体类，主要定义一个实体模型，通过`TableAttribute`特性指定表格名称，也可以使用`TagetAttribute`指定对应的类型表格名称，其他主要使用到的特性如下：

1. TableAttribute, TargetAttribute：表格特性，标注于类型定义；
2. KeyAttribute：设置主键列，在数据库操作中如果为唯一列为主键，可以通过主键实体对象进行查询，更新，删除等等操作；
3. IdentityAttribute：自增长列，如果实体类中没有特别定义的主键列，当前列将会被定义为主键列；
4. SizeAttribute：长度，比如在字符串中或者其他可变长度的时候需要设置长度值，在SQLServer中，如果字符串类型不设置，则使用`nvarchar(MAX)`类型；
5. NotMappedAttribute：忽略当前属性，标注的属性将不会自动匹配查询，更新等等操作；
6. NotUpdatedAttribute：在更新整个实体时候，将忽略当前标注的属性，比如，添加时间字段。
7. NumberAttribute：在需要设置精度系数的情况下使用，保护总位数和小数点位数。

```csharp
    /// <summary>
    /// 数据库实例。
    /// </summary>
    [Table("core_Migrations")]
    public class Migration
    {
        /// <summary>
        /// 迁移类型。
        /// </summary>
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 版本。
        /// </summary>
        public int Version { get; set; }
    }
```

迁移类，在定义好一个实体类型后，需要将它的数据结构和数据库进行同步，在Gentings的数据库迁移模块中，需要手动添加一个数据库迁移类。数据库迁移类需要继承自`DataMigration`，通过`MigrationBuilder`实例来修改或者创建数据库等等操作，具体代码如下：

```csharp
    /// <summary>
    /// 数据库迁移表格迁移类型。
    /// </summary>
    public class CoreDataMigration : DataMigration
    {
        /// <summary>
        /// 当模型建立时候构建的表格实例。
        /// </summary>
        /// <param name="builder">迁移实例对象。</param>
        public override void Create(MigrationBuilder builder)
        {
            builder.CreateTable<Migration>(table => table
                .Column(c => c.Id)
                .Column(c => c.Version)
            );
        }
    }
```

这样Gentings框架将根据迁移类进行数据表的操作。如果需要修改表格定义，则可以在数据库迁移类中定义升级方法`Up`，也可以进行降级方法`Down`跟着版本号，例如：

```csharp
        /// <summary>
        /// 添加默认角色。
        /// </summary>
        /// <param name="builder">数据迁移构建实例。</param>
        public void Up1(MigrationBuilder builder)
        {
            var role1 = new UserRole { RoleId = 1, UserId = 1 };
            builder.SqlCreate(role1);
            var role2 = new UserRole { RoleId = 2, UserId = 1 };
            builder.SqlCreate(role2);
        }
```

> 需要注意的是，版本只可以往上增加，添加代码之后不要移除已经更新后的版本号码，因为数据库会永久记录当前迁移的版本。

## IDatabase

这个接口中包含了数据库ADO.NET的基础操作，主要包含了三个方法：`ExecuteNonQuery`，`ExecuteReader`，`ExecuteScalar`，当然也对数据库事务操作进行了封装：`BeginTransaction`。

在数据库操作中，还对各自的异步方法进行了封装，如果在开发阶段，只要数据库执行有出现任何问题，我们都可以在**调试输出**窗口中查看到错误记录，以及当前执行的SQL语句。

## IDbContext<TModel>

这个接口主要对相应的模型对象进行操作，包含了每个实体类的增加(Create)、读取查询(Retrieve)、更新(Update)和删除(Delete)，还封装了判断存在Any，分页查询等等。

所有操作的条件语句都可以通过Lambda表达式来描述，Gentings数据库模块将会自动转换表达式为SQL语句，对于添加和更新复杂的语句Gentings将会对其缓存，以提升相应的性能。

除了基础的操作外，我们还提供了多表关联查询的接口`IQueryable<TModel>`接口，可以对多个表格进行关联查询操作。

同时也对事务进行的支持，特别在SQLServer数据库操作中，在启动一个事务时候，将调用的对象为当前事务的`IDbTransactionContext<TModel>`实例对象，需要注意的是在事务中如果使用异步事务，所有调用的方法都需要异步，否则可能出现不可预知的错误，从而达不到操作目的。

> 注：Gentings的数据库操作抽象框架，不同的数据库需要单独实现数据库具体操作，暂时提供SqlServer数据库的封装，所以文档中如果没有特殊说明，将直接针对的是SQLServer数据库。
