﻿using Knowledge_Graph_Analysis_BackEnd.Dtos;
using Knowledge_Graph_Analysis_BackEnd.IRepositories;

namespace Knowledge_Graph_Analysis_BackEnd.Services.Implements
{
    public class AuthorServiceImpl : IAuthorService
    {
        private readonly IAuthorRepository authorRepository;

        public AuthorServiceImpl(IAuthorRepository authorRepository)
        {
            this.authorRepository = authorRepository;
        }

        public async Task<List<BriefAuthor>> GetAuthorsBriefInfoByName(string name)
        {
            List<BriefAuthor> authors = new List<BriefAuthor>();
            List<string> authorIndexs = await authorRepository.GetAuthorIndexByName(name);
            if(authorIndexs != null)
            {
                foreach (string authorIndex in authorIndexs)
                {              
                    List<string> authorDepartments = await authorRepository.GetAuthorDepartment(authorIndex);
                    List<string> authorAreas = await authorRepository.GetAuthorAreas(authorIndex);
                    List<string> authorPapers = await authorRepository.GetAuthorPaperTitle(authorIndex);
                    BriefAuthor briefAuthor = new BriefAuthor(authorIndex, authorDepartments, authorPapers, authorAreas);
                    authors.Add(briefAuthor);
                }
            }
            return authors;
        }
    }
}
