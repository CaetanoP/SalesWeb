import { Loader2 } from 'lucide-react';
import {
  AlertDialog,
  AlertDialogAction,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle
} from './ui/alert-dialog';

export function AreYouSure({
  description,
  open,
  setOpen,
  onConfirm,
  loading
}: {
  description: string;
  open: boolean;
  setOpen: (open: boolean) => void;
  onConfirm: () => void;
  loading?: boolean;
}) {
  return (
    <>
      <AlertDialog open={open} onOpenChange={setOpen}>
        <AlertDialogContent className="bg-slate-100">
          <AlertDialogHeader>
            <AlertDialogTitle>Are you absolutely sure?</AlertDialogTitle>
            <AlertDialogDescription>{description}</AlertDialogDescription>
          </AlertDialogHeader>
          <AlertDialogFooter>
            <AlertDialogCancel>Cancel</AlertDialogCancel>
            <AlertDialogAction
              onClick={() => onConfirm()}
              disabled={loading}
              className="transition duration-500"
            >
              Continue
            </AlertDialogAction>
          </AlertDialogFooter>
          <div
            data-loading={loading}
            className="data-[loading=true]:h-7 h-0 transition-all duration-300 ease-linear rounded-b-md inset-x-2 overflow-hidden absolute top-[calc(100%+1px)] flex bg-primary -z-10 text-white justify-end items-center"
          >
            <span className="me-3 text-sm">Loading...</span>
            <Loader2 className="animate-spin w-4 h-4 me-3" />
          </div>
        </AlertDialogContent>
      </AlertDialog>
    </>
  );
}
