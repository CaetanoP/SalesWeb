'use client';

import { Button } from '@/components/ui/button';
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger
} from '@/components/ui/dialog';
import {
  Form,
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { zodResolver } from '@hookform/resolvers/zod';
import { PlusCircle } from 'lucide-react';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { toast } from 'sonner';
import { z } from 'zod';
import { addDepartment } from './actions';

const formSchema = z.object({
  name: z.string().min(1, 'Name is required').max(20, 'Name is too long')
});

export function DepartmentsNewForm() {
  const [open, setOpen] = useState(false);
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      name: ''
    }
  });

  function onSubmit(values: z.infer<typeof formSchema>) {
    try {
      addDepartment(values.name);
      setOpen(false);
      form.reset();
    } catch (error) {
      console.log(error);
      toast.error('Error adding department');
    }
    toast.success('Department added successfully');
    console.log(values);
  }

  return (
    <Dialog open={open} onOpenChange={(open) => setOpen(open)}>
      <DialogTrigger asChild>
        <Button size="sm" className="h-8 gap-1" onClick={() => setOpen(true)}>
          <PlusCircle className="h-3.5 w-3.5" />
          <span className="sr-only sm:not-sr-only sm:whitespace-nowrap">
            Add Department
          </span>
        </Button>
      </DialogTrigger>
      <DialogContent className="sm:max-w-[425px]">
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)}>
            <DialogHeader>
              <DialogTitle>Add New Deparment</DialogTitle>
              <DialogDescription>
                Add a new Department to register Sellers and Sales.
              </DialogDescription>
            </DialogHeader>
            <FormField
              control={form.control}
              name="name"
              render={({ field }) => (
                <div className="grid gap-4 py-4">
                  <FormItem>
                    <div className="grid grid-cols-4 items-center gap-x-4 gap-y-1">
                      <FormLabel htmlFor="name" className="text-right">
                        Name
                      </FormLabel>
                      <FormControl>
                        <Input
                          placeholder="DepartmentName"
                          className="col-span-3"
                          {...field}
                        />
                      </FormControl>
                      <FormMessage className="col-span-3 col-start-2" />
                    </div>
                  </FormItem>
                </div>
              )}
            ></FormField>
            <DialogFooter>
              {/* <DialogClose asChild> */}
              <Button type="submit">Add</Button>
              {/* </DialogClose> */}
            </DialogFooter>
          </form>
        </Form>
      </DialogContent>
    </Dialog>
  );
}
