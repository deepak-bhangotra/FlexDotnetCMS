﻿using System.Collections.Generic;
using System.Linq;

namespace FrameworkLibrary
{
    public partial class Media : IMustContainID
    {
        public string GetTagsAsString()
        {
            return MediaTags.Select(i => i.Tag).Aggregate("", (current, item) => current + (item.Name + ","));
        }

        public Return MoveUp()
        {
            return MoveToIndex(GetIndex() - 1);
        }

        public Return MoveDown()
        {
            return MoveToIndex(GetIndex() + 1);
        }

        public Return MoveToIndex(int insertAtIndex)
        {
            var returnObj = BaseMapper.GenerateReturn();

            if (insertAtIndex < 0)
            {
                var ex = new System.Exception("Cant move to index: " + insertAtIndex);
                returnObj.Error = ErrorHelper.CreateError(ex);

                return returnObj;
            }

            var siblings = GetSiblings();
            var currentIndex = GetIndex(siblings);

            if (currentIndex >= 0 && currentIndex <= siblings.Count)
                siblings.RemoveAt(currentIndex);

            if (siblings.Count < insertAtIndex)
            {
                var ex = new System.Exception("Cant move to index: " + insertAtIndex);
                returnObj.Error = ErrorHelper.CreateError(ex);

                return returnObj;
            }

            siblings.Insert(insertAtIndex, this);

            var index = 0;

            foreach (var media in siblings)
            {
                index++;

                if (media.OrderIndex == index - 1)
                    continue;

                var inContext = BaseMapper.GetObjectFromContext(media);
                media.OrderIndex = index - 1;
                returnObj = MediasMapper.Update(media);
            }

            MediasMapper.ClearCache();
            MediaDetailsMapper.ClearCache();

            return returnObj;
        }

        public List<Media> GetSiblings()
        {
            return this.ParentMedia.ChildMedias.OrderBy(i => i.OrderIndex).ToList();
        }

        public int GetIndex()
        {
            var siblings = GetSiblings();
            return siblings.FindIndex(i => i.ID == this.ID);
        }

        public int GetIndex(List<Media> inList)
        {
            return inList.FindIndex(i => i.ID == this.ID);
        }

        public void ReorderChildren()
        {
            var index = 0;
            foreach (var mediaItem in ChildMedias.OrderBy(i => i.OrderIndex))
            {
                var context = BaseMapper.GetObjectFromContext(mediaItem);

                if (context == null)
                    continue;

                if (context.OrderIndex != index)
                {
                    context.OrderIndex = index;
                    MediasMapper.Update(context);
                }

                index++;
            }
        }
    }
}