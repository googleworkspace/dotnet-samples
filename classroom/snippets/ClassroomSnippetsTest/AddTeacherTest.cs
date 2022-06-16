// Copyright 2022 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using NUnit.Framework;
using ClassroomSnippets;

 namespace ClassroomSnippetsTest;

 public class AddTeacherTest : BaseTest
 {
   [Test]
   public void TestAddTeacher()
   {
     var teacher = AddTeacher.ClassroomAddTeacher( this.TestCourse.Id, this.OtherUser);
     Assert.IsNotNull(teacher, "Teacher not returned.");
     Assert.AreEqual(this.TestCourse.Id, teacher.CourseId, "Teacher added to wrong course.");
   }
 }